using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BloqueioPrimeiraNoite : MonoBehaviour
{
    [TextArea(2, 4)]
    [SerializeField] private string mensagemBloqueio = "Melhor não ir para o lado da mansão agora. Está muito escuro nesta primeira noite.";

    [SerializeField] private float intervaloMensagem = 2.5f;
    [Tooltip("Desmarque se o bloqueio estiver no lado ESQUERDO da praça; marque se estiver no lado DIREITO")]
    [SerializeField] private bool bloquearParaDireita = false;

    private float ultimoAviso = -10f;
    private Collider2D colisorBloqueio;
    private Transform playerTransform;

    private void Awake()
    {
        colisorBloqueio = GetComponent<Collider2D>();
        colisorBloqueio.isTrigger = true;
    }

    private void LateUpdate()
    {
        // Só roda se este objeto estiver ativo na sala atual (activeInHierarchy) e for a Primeira Noite
        if (!gameObject.activeInHierarchy) return;
        if (GameStateManager.Instance == null || GameStateManager.Instance.etapaDoDia != 1) return;

        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
            return;
        }

        Vector3 pos = playerTransform.position;

        if (bloquearParaDireita)
        {
            float limite = colisorBloqueio.bounds.min.x;
            if (pos.x > limite)
            {
                pos.x = limite;
                playerTransform.position = pos;
                MostrarAviso();
            }
        }
        else
        {
            float limite = colisorBloqueio.bounds.max.x;
            if (pos.x < limite)
            {
                pos.x = limite;
                playerTransform.position = pos;
                MostrarAviso();
            }
        }
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