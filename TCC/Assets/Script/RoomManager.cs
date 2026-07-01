using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Referências da Cena")]
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private Transform playerTransform;

    [Header("Banco de Dados de Salas")]
    [SerializeField] private List<RoomData> allRooms;

    private Dictionary<string, RoomData> roomDatabase = new Dictionary<string, RoomData>();
    private RoomData currentRoom;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerMovement = playerTransform.GetComponent<PlayerMovement>();

        // Indexa as salas para busca rápida por ID
        foreach (var room in allRooms)
        {
            if (!roomDatabase.ContainsKey(room.roomID))
                roomDatabase.Add(room.roomID, room);
        }
    }

    private void Start()
    {
        // Carrega a primeira sala do banco de dados como teste (ou passe uma padrão)
        if (allRooms.Count > 0)
        {
            LoadRoom(allRooms[0].roomID, allRooms[0].spawnPoints[0].key);
        }
    }

    // Substitua o método Update do seu RoomManager por este:
    private void Update()
    {
        if (currentRoom == null) return;

        float playerX = playerTransform.position.x;

        foreach (var transition in currentRoom.transitions)
        {
            if (Mathf.Abs(playerX - transition.triggerX) <= transition.range)
            {
                // Lógica para o Novo Input System detectar a Barra de Espaço
                if (UnityEngine.InputSystem.Keyboard.current != null &&
                    UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
                {
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

        // 1. Troca o Cenário Visual (Hot-Swap)
        backgroundRenderer.sprite = currentRoom.backgroundSprite;

        // 2. Atualiza os limites de movimento do jogador nesta sala
        playerMovement.SetLimits(currentRoom.minX, currentRoom.maxX);

        // 3. Posiciona o jogador no Spawn Point correto
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

        playerTransform.position = new Vector3(spawnX, playerTransform.position.y, playerTransform.position.z);
        playerMovement.StopMovement(); // Para o clique anterior para o player não continuar andando sozinho
    }
}