using UnityEngine;

// Redireciona todas as mensagens de examinar, coletar e avisos
// para a caixa principal do DialogoManager (com efeito de digitação).
public class MensagemUI : MonoBehaviour
{
    public static bool Aberta => DialogoManager.Aberto;

    public static void Mostrar(string mensagem, string titulo = "")
    {
        if (string.IsNullOrWhiteSpace(mensagem)) return;

        if (DialogoManager.Instance != null)
        {
            LinhaDialogo[] linhaUnica = new LinhaDialogo[]
            {
                new LinhaDialogo
                {
                    nomeFalante = titulo,
                    retrato = null,
                    fala = mensagem
                }
            };

            DialogoManager.Instance.IniciarDialogo(linhaUnica);
        }
        else
        {
            Debug.LogWarning("DialogoManager.Instance não encontrado na cena para exibir: " + mensagem);
        }
    }

    public static void Fechar()
    {
        // O próprio DialogoManager fecha a caixa ao terminar a leitura
    }
}