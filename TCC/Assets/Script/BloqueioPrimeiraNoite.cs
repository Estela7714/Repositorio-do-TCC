using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BloqueioPrimeiraNoite : MonoBehaviour
{
    [TextArea(2, 4)]
    [SerializeField] private string mensagemBloqueio = "Melhor não ir para o lado da mansão agora. Está muito escuro nesta primeira noite.";

    [SerializeField] private float intervaloMensagem = 2.5f;
    private float ultimoAviso = -10f;

    private bool AindaEhPrimeiraNoite =>
        GameStateManager.Instance != null && GameStateManager.Instance.etapaDoDia == 1;

    private void Start()
    {
        AtualizarBloqueio();
    }

    private void OnEnable()
    {
        AtualizarBloqueio();
    }

    private void Update()
    {
        if (!AindaEhPrimeiraNoite)
        {
            gameObject.SetActive(false);
        }
    }

    public void AtualizarBloqueio()
    {
        gameObject.SetActive(AindaEhPrimeiraNoite);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (AindaEhPrimeiraNoite && collision.gameObject.CompareTag("Player"))
            MostrarAviso();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (AindaEhPrimeiraNoite && other.CompareTag("Player"))
            MostrarAviso();
    }

    private void MostrarAviso()
    {
        if (Time.time - ultimoAviso >= intervaloMensagem)
        {
            ultimoAviso = Time.time;
            MensagemUI.Mostrar(mensagemBloqueio);
        }
    }
}