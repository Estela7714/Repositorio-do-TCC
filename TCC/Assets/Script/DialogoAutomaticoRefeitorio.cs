using UnityEngine;
using UnityEngine.Events;

public class DialogoAutomaticoRefeitorio : MonoBehaviour
{
    [Header("Condição")]
    [Tooltip("Marque como verdadeiro quando o jogador terminar de falar com a mulher da praça")]
    public static bool falouComMulherPraca = false;

    [Header("Visibilidade do Player")]
    [Tooltip("Arraste o objeto do Player aqui (se deixar vazio, ele busca pela Tag 'Player')")]
    public GameObject objetoPlayer;

    [Header("Ação Automática")]
    [Tooltip("Arraste aqui a função que inicia o diálogo deste NPC")]
    public UnityEvent onIniciarDialogoAutomatico;

    private bool jaAconteceu = false;
    private SpriteRenderer[] spritesPlayer;

    private void OnEnable()
    {
        if (!falouComMulherPraca || jaAconteceu) return;

        jaAconteceu = true;
        DefinirVisibilidadePlayer(false);

        // Pequeno atraso de 0.2s para garantir que a transição de sala e UI terminaram
        Invoke(nameof(DispararDialogo), 0.2f);
    }

    private void DispararDialogo()
    {
        if (DialogoManager.Aberto || MensagemUI.Aberta)
        {
            Invoke(nameof(DispararDialogo), 0.2f);
            return;
        }

        onIniciarDialogoAutomatico?.Invoke();
    }

    // Chame esta função no evento 'Ao Terminar Conversa ()' do NPC da mulher da praça
    public void LiberarEventoRefeitorio()
    {
        falouComMulherPraca = true;
    }

    // Chame esta função no evento 'Ao Terminar Conversa ()' do NPC da mesa do refeitório
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