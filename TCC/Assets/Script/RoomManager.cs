using System.Collections.Generic;
using UnityEngine;

// Liga o ID de uma sala ao objeto da cena que guarda os interagíveis dela
[System.Serializable]
public class SalaObjetos
{
    public string roomID;
    public GameObject raiz; // objeto pai com todos os interagíveis desta sala
}

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Referências da Cena")]
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;

    [Header("Ajuste do Fundo")]
    [Tooltip("Ligado: preenche a tela toda (pode cortar bordas). Desligado: mostra a imagem inteira (pode sobrar faixa).")]
    [SerializeField] private bool preencherTela = true;

    [Header("Banco de Dados de Salas")]
    [SerializeField] private List<RoomData> allRooms;

    [Header("Objetos Interagíveis por Sala")]
    [SerializeField] private List<SalaObjetos> objetosDasSalas;

    [Header("Bloqueio Antes de Falar com a Atendente")]
    [TextArea(2, 4)]
    [SerializeField] private string mensagemBloqueioAtendente = "Melhor falar com a atendente da pousada primeiro antes de sair explorando a cidade.";

    [Header("Bloqueio da Delegacia (Dia 1)")]
    [Tooltip("Coloque o Target Room ID exato da delegacia configurado no RoomData.")]
    [SerializeField] private string idSalaDelegacia = "delegacia";
    [TextArea(2, 4)]
    [SerializeField] private string mensagemBloqueioDelegacia = "A delegacia está fechada esta noite. Melhor voltar amanhã.";

    [Header("Bloqueio da Delegacia (Antes de Falar com a Senhora no Dia 2)")]
    [TextArea(2, 4)]
    [SerializeField] private string mensagemBloqueioSenhoraDia2 = "Preciso falar com a senhora antes de ir até a delegacia.";

    private Dictionary<string, RoomData> roomDatabase = new Dictionary<string, RoomData>();
    private RoomData currentRoom;
    private PlayerMovement playerMovement;
    private float defaultPlayerY;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerMovement = playerTransform.GetComponent<PlayerMovement>();
        defaultPlayerY = playerTransform.position.y;

        if (mainCamera == null) mainCamera = Camera.main;

        foreach (var room in allRooms)
        {
            if (!roomDatabase.ContainsKey(room.roomID))
                roomDatabase.Add(room.roomID, room);
        }
    }

    private void Start()
    {
        if (allRooms.Count > 0)
        {
            LoadRoom(allRooms[0].roomID, allRooms[0].spawnPoints[0].key);
        }
    }

    private void Update()
    {
        if (currentRoom == null) return;

        // Não troca de sala com mensagem ou diálogo aberto
        if (MensagemUI.Aberta || DialogoManager.Aberto) return;

        float playerX = playerTransform.position.x;

        foreach (var transition in currentRoom.transitions)
        {
            if (Mathf.Abs(playerX - transition.triggerX) <= transition.range)
            {
                if (UnityEngine.InputSystem.Keyboard.current != null &&
                    UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    if (MansaoStateManager.Instance != null &&
                        MansaoStateManager.Instance.EstaBloqueadoNaPortaPrincipal(currentRoom.roomID, playerX))
                    {
                        break;
                    }

                    // Impede ir para a praça ou para o beco antes de falar com a atendente da pousada
                    if (!BloqueioAtendentePousada.jaFalouComAtendente &&
                        (transition.targetRoomID == "praca_central" || transition.targetRoomID == "beco_escuro"))
                    {
                        MensagemUI.Mostrar(mensagemBloqueioAtendente);
                        break;
                    }

                    // Impede ir para a delegacia antes do Dia 2
                    if (transition.targetRoomID == idSalaDelegacia &&
                        (GameStateManager.Instance == null || GameStateManager.Instance.etapaDoDia < 2))
                    {
                        MensagemUI.Mostrar(mensagemBloqueioDelegacia);
                        break;
                    }

                    // Impede ir para a delegacia no Dia 2 antes de falar com a senhora
                    if (transition.targetRoomID == idSalaDelegacia &&
                        !ControleAcessoDelegacia.FalouComSenhoraDia2)
                    {
                        MensagemUI.Mostrar(mensagemBloqueioSenhoraDia2);
                        break;
                    }

                    LoadRoom(transition.targetRoomID, transition.targetSpawnKey);
                    break;
                }
            }
        }
    }

    public void LoadRoom(string roomId, string spawnKey)
    {
        if (!roomDatabase.ContainsKey(roomId))
        {
            Debug.LogError($"Sala com ID {roomId} não encontrada!");
            return;
        }

        currentRoom = roomDatabase[roomId];

        // 1. Troca o cenário visual
        backgroundRenderer.sprite = currentRoom.backgroundSprite;
        FitBackgroundToCamera();

        // 1.1 Ativa só os objetos interagíveis da sala atual
        AtualizarObjetosDaSala(roomId);

        // 2. Limites de movimento
        playerMovement.SetLimits(currentRoom.minX, currentRoom.maxX);

        // 3. Spawn do jogador
        float spawnX = 0f;
        bool foundSpawn = false;
        foreach (var spawn in currentRoom.spawnPoints)
        {
            if (spawn.key == spawnKey)
            {
                spawnX = spawn.xPosition;
                foundSpawn = true;
                break;
            }
        }

        if (!foundSpawn) Debug.LogWarning($"SpawnPoint '{spawnKey}' não encontrado na sala {roomId}. Usando X zero.");

        // 4. Ajuste de Escala e Altura (Y) por Sala
        float escala = currentRoom.playerScale > 0f ? currentRoom.playerScale : 1f;
        float direcaoX = Mathf.Sign(playerTransform.localScale.x);
        if (direcaoX == 0f) direcaoX = 1f;
        playerTransform.localScale = new Vector3(escala * direcaoX, escala, 1f);

        float targetY = currentRoom.usarCustomY ? currentRoom.playerY : defaultPlayerY;
        playerTransform.position = new Vector3(spawnX, targetY, playerTransform.position.z);
        playerMovement.StopMovement();
    }

    private void AtualizarObjetosDaSala(string roomId)
    {
        foreach (var sala in objetosDasSalas)
        {
            if (sala.raiz != null)
                sala.raiz.SetActive(sala.roomID == roomId);
        }
    }

    [Header("Tamanho Fixo do Cenário (Independente de Monitor)")]
    [Tooltip("Largura fixa em unidades do mundo que toda sala deve ter (ex: 19.2 para 1920px a 100 PPU).")]
    [SerializeField] private float larguraFixaCenario = 19.2f;

    private void FitBackgroundToCamera()
    {
        Sprite sprite = backgroundRenderer.sprite;
        if (sprite == null) return;

        // Mantém sempre a MESMA escala do cenário em qualquer PC/resolução
        float scale = larguraFixaCenario / sprite.bounds.size.x;
        backgroundRenderer.transform.localScale = new Vector3(scale, scale, 1f);

        // Trava o cenário sempre no centro exato do mundo (0, 0)
        backgroundRenderer.transform.position = new Vector3(0f, 0f, backgroundRenderer.transform.position.z);
    }

    public void EntrarPortaFrenteMansao()
    {
        LoadRoom("mansao_porta_frente", "vindo_portao");
    }

    public void EntrarEscritorioDuque()
    {
        LoadRoom("mansao_escritorio_duque", "esquerda");
    }
}