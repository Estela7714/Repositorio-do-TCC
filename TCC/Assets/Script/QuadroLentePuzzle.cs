using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuadroLentePuzzle : MonoBehaviour
{
    [Header("Item Necessário")]
    [Tooltip("Arraste o ItemData da Lente de Vidro Vermelho")]
    public ItemData itemLenteVermelha;
    [Tooltip("Marque apenas se quiser que a lente suma do inventário após usar")]
    public bool consumirLenteAoUsar = false;

    [Header("Painel / Imagem Revelada (UI)")]
    [Tooltip("Arraste a Image ou Painel do Canvas que mostra o quadro visto pela lente")]
    public GameObject painelQuadroRevelado;
    public Image imagemQuadroUI;
    public Sprite spriteQuadroComLente;

    [Header("Mensagens")]
    [TextArea] public string mensagemSemLente = "Uma pintura antiga e desbotada. Há rabiscos estranhos sob a tinta, mas não consigo distinguir nada a olho nu.";
    [TextArea] public string mensagemComLente = "Ao olhar através da Lente de Vidro Vermelho, símbolos ocultos surgem na tela!";

    [Header("Evento Opcional")]
    public UnityEvent onQuadroRevelado;

    private bool jaRevelado = false;

    private void Start()
    {
        if (painelQuadroRevelado != null)
            painelQuadroRevelado.SetActive(false);
    }

    // Ligue este método no 'Ao Interagir ()' ou 'Ao Usar Item Correto ()' do ObjetoInteragivel do Quadro
    public void InteragirComQuadro()
    {
        // Trava: enquanto o evento da lareira não estiver completo, ignora a lente para o evento da lareira ir primeiro
        if (EventoAticadorLareira.Instance != null && !EventoAticadorLareira.Instance.EventoConcluido)
        {
            return;
        }

        bool possuiLente = Inventory.Instance != null
                           && itemLenteVermelha != null
                           && Inventory.Instance.Possui(itemLenteVermelha.itemID);

        if (!jaRevelado && !possuiLente)
        {
            return;
        }

        if (!jaRevelado)
        {
            jaRevelado = true;

            if (consumirLenteAoUsar)
                Inventory.Instance.Remover(itemLenteVermelha);

            onQuadroRevelado?.Invoke();
        }

        AbrirImagemRevelada(mensagemComLente);
    }

    private void AbrirImagemRevelada(string mensagem)
    {
        if (painelQuadroRevelado != null)
        {
            if (imagemQuadroUI != null && spriteQuadroComLente != null)
                imagemQuadroUI.sprite = spriteQuadroComLente;

            painelQuadroRevelado.SetActive(true);
        }

        if (!string.IsNullOrEmpty(mensagem))
            MensagemUI.Mostrar(mensagem);
    }

    public void FecharImagemRevelada()
    {
        if (painelQuadroRevelado != null)
            painelQuadroRevelado.SetActive(false);
    }
}