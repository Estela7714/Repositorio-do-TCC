using UnityEngine;

public class PortaTrancadaIlusao : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private LinhaDialogo[] falasTrancada;
    public KeyCode teclaInteracao = KeyCode.Space;

    private bool jogadorPerto = false;

    private void Start()
    {
        // Se já entrar na sala com o boss derrotado, desativa o bloqueio imediatamente
        if (MansaoStateManager.Instance != null && MansaoStateManager.Instance.bossDerrotado)
        {
            gameObject.SetActive(false);
            return;
        }

        // Escuta o evento de morte do boss para destrancar automaticamente
        if (MansaoStateManager.Instance != null)
        {
            MansaoStateManager.Instance.OnMansaoEstadoMudou += VerificarDestrancamento;
        }
    }

    private void OnDestroy()
    {
        if (MansaoStateManager.Instance != null)
        {
            MansaoStateManager.Instance.OnMansaoEstadoMudou -= VerificarDestrancamento;
        }
    }

    private void VerificarDestrancamento()
    {
        if (MansaoStateManager.Instance.bossDerrotado)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (DialogoManager.Aberto || MensagemUI.Aberta) return;

        // Intercepta a tecla antes que o RoomManager troque de sala
        if (jogadorPerto && Input.GetKeyDown(teclaInteracao))
        {
            if (DialogoManager.Instance != null && falasTrancada != null && falasTrancada.Length > 0)
            {
                DialogoManager.Instance.IniciarDialogo(falasTrancada);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
        }
    }
}