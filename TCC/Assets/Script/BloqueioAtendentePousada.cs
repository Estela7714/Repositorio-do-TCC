using UnityEngine;

public class BloqueioAtendentePousada : MonoBehaviour
{
    public static bool jaFalouComAtendente = false;

    // Ligue este método no UnityEvent "Ao Terminar Conversa ()" da Atendente da Pousada
    public void LiberarCaminhos()
    {
        jaFalouComAtendente = true;
    }
}