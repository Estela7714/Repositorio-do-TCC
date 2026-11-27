using UnityEngine;

public class BloqueioAtendentePousada : MonoBehaviour
{
    public static bool jaFalouComAtendente = false;

    [Header("Teste no Editor")]
    [Tooltip("Marque esta caixa no Inspector antes de dar Play para começar com as barreiras já desativadas.")]
    public bool ignorarBarreiraNoEditor = false;

    private void Awake()
    {
        if (ignorarBarreiraNoEditor)
        {
            jaFalouComAtendente = true;
        }
    }

    // Ligue este método no UnityEvent "Ao Terminar Conversa ()" da Atendente da Pousada
    public void LiberarCaminhos()
    {
        jaFalouComAtendente = true;
    }

    // Botão extra no menu dos três pontinhos (⋮) do componente no Inspector
    [ContextMenu("Desativar Barreiras Agora")]
    public void DesativarBarreirasPeloMenu()
    {
        ignorarBarreiraNoEditor = true;
        jaFalouComAtendente = true;
        Debug.Log("Barreiras da Atendente desativadas para teste!");
    }
}