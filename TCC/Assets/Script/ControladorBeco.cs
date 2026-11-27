using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ControladorBeco : MonoBehaviour
{
    [Header("NPCs do Beco")]
    [Tooltip("Arraste aqui os NPCs do beco que só aparecem no Dia 1")]
    public GameObject[] npcsBeco;

    [Header("Visibilidade do Player")]
    [Tooltip("Arraste o objeto do Player aqui (se deixar vazio, ele busca pela Tag 'Player')")]
    public GameObject objetoPlayer;

    [Header("Ação Automática")]
    [Tooltip("Arraste aqui a função que inicia o diálogo dos moradores do beco")]
    public UnityEvent onIniciarDialogoAutomatico;

    private bool jaAconteceu = false;
    private bool aguardandoFimDialogo = false;
    private SpriteRenderer[] spritesPlayer;

    private void OnEnable()
    {
        int etapa = GameStateManager.Instance != null ? GameStateManager.Instance.etapaDoDia : 1;
        bool ehDia1 = (etapa == 1);

        // 1. Controla se os NPCs do beco existem na cena (só no Dia 1)
        foreach (GameObject npc in npcsBeco)
        {
            if (npc != null)
                npc.SetActive(ehDia1);
        }

        // 2. Ignora o momento em que a cena acaba de abrir (boot do jogo)
        if (Time.timeSinceLevelLoad < 0.5f) return;

        // 3. Se o jogador realmente entrou no beco agora no Dia 1, inicia a cutscene
        if (ehDia1 && !jaAconteceu)
        {
            StartCoroutine(RotinaEntradaBeco());
        }
    }

    private IEnumerator RotinaEntradaBeco()
    {
        // Espera 1 frame para confirmar que o RoomManager não desativou esta sala no início
        yield return null;

        if (!gameObject.activeInHierarchy || jaAconteceu) yield break;

        jaAconteceu = true;
        DefinirVisibilidadePlayer(false);

        yield return new WaitForSeconds(0.2f);

        while (DialogoManager.Aberto || MensagemUI.Aberta)
        {
            yield return new WaitForSeconds(0.2f);
        }

        onIniciarDialogoAutomatico?.Invoke();
        aguardandoFimDialogo = true;
    }

    private void Update()
    {
        if (aguardandoFimDialogo && !DialogoManager.Aberto)
        {
            aguardandoFimDialogo = false;
            MostrarPlayerNovamente();
        }
    }

    public void MostrarPlayerNovamente()
    {
        DefinirVisibilidadePlayer(true);
    }

    private void DefinirVisibilidadePlayer(bool visivel)
    {
        if (objetoPlayer == null)
        {
            GameObject encontrado = GameObject.FindGameObjectWithTag("Player");
            if (encontrado != null) objetoPlayer = encontrado;
        }

        if (objetoPlayer == null) return;

        if (spritesPlayer == null || spritesPlayer.Length == 0)
            spritesPlayer = objetoPlayer.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sr in spritesPlayer)
        {
            if (sr != null)
                sr.enabled = visivel;
        }
    }
}