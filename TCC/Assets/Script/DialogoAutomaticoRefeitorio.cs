using UnityEngine;
using UnityEngine.Events;

public class DialogoAutomaticoRefeitorio : MonoBehaviour
{
    [Header("Condição")]
    [Tooltip("Marque como verdadeiro quando o jogador terminar de falar com a mulher da praça")]
    public static bool falouComMulherPraca = false;

    [Header("Ação Automática")]
    [Tooltip("Arraste aqui a função que inicia o diálogo deste NPC")]
    public UnityEvent onIniciarDialogoAutomatico;

    private bool jaAconteceu = false;

    private void OnEnable()
    {
        if (!falouComMulherPraca || jaAconteceu) return;

        jaAconteceu = true;
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

    // Chame esta função no evento de fim de diálogo da mulher da praça
    public void LiberarEventoRefeitorio()
    {
        falouComMulherPraca = true;
    }
}