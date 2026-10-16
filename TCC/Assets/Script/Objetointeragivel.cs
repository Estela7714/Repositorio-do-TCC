using UnityEngine;
using UnityEngine.Events;

public enum TipoInteracao
{
    Examinar, // Clicar mostra um texto (pistas, informações)
    Coletar   // Clicar adiciona um item ao inventário e some da cena
}

// Coloque em qualquer objeto da cena que tenha um Collider2D.
[RequireComponent(typeof(Collider2D))]
public class ObjetoInteragivel : MonoBehaviour
{
    [Header("Geral")]
    [SerializeField] private string nomeObjeto;
    [SerializeField] private TipoInteracao tipo = TipoInteracao.Examinar;

    [Header("Clique normal (sem item selecionado)")]
    [TextArea(2, 5)][SerializeField] private string textoExaminar;
    [SerializeField] private ItemData itemParaColetar; // usado se tipo = Coletar
    [TextArea(2, 5)][SerializeField] private string textoAoColetar;
    [SerializeField] private UnityEvent aoInteragir;

    [Header("Uso de item (item selecionado no inventário)")]
    [Tooltip("Deixe vazio se este objeto não aceita nenhum item.")]
    [SerializeField] private ItemData itemNecessario;
    [TextArea(2, 5)][SerializeField] private string textoItemCorreto;
    [TextArea(2, 5)][SerializeField] private string textoItemErrado = "Isso não funciona aqui.";
    [TextArea(2, 5)][SerializeField] private string textoDepoisDeUsado;
    [SerializeField] private bool consumirItem = true;
    [SerializeField] private bool usoUnico = true;
    [Tooltip("Item entregue ao jogador quando o uso der certo (opcional).")]
    [SerializeField] private ItemData itemRecompensa;
    [Tooltip("Ex: ativar um objeto escondido, abrir passagem, liberar pista...")]
    [SerializeField] private UnityEvent aoUsarItemCorreto;

    private bool jaUsado;

    public string NomeObjeto => nomeObjeto;

    // Clique com a "mão vazia"
    public void Interagir()
    {
        if (jaUsado && !string.IsNullOrWhiteSpace(textoDepoisDeUsado))
        {
            MensagemUI.Mostrar(textoDepoisDeUsado);
            return;
        }

        if (tipo == TipoInteracao.Coletar && itemParaColetar != null)
        {
            Inventory.Instance.Adicionar(itemParaColetar);

            string msg = string.IsNullOrWhiteSpace(textoAoColetar)
                ? $"Peguei: {itemParaColetar.nomeItem}."
                : textoAoColetar;
            MensagemUI.Mostrar(msg);

            aoInteragir?.Invoke();
            gameObject.SetActive(false); // continua desativado mesmo ao trocar de sala
            return;
        }

        MensagemUI.Mostrar(textoExaminar);
        aoInteragir?.Invoke();
    }

    // Clique com um item selecionado no inventário
    public void UsarItem(ItemData item)
    {
        if (item == null) return;

        if (usoUnico && jaUsado)
        {
            MensagemUI.Mostrar(string.IsNullOrWhiteSpace(textoDepoisDeUsado) ? textoExaminar : textoDepoisDeUsado);
            return;
        }

        if (itemNecessario != null && item.itemID == itemNecessario.itemID)
        {
            jaUsado = true;
            MensagemUI.Mostrar(textoItemCorreto);

            if (consumirItem) Inventory.Instance.Remover(item);
            if (itemRecompensa != null) Inventory.Instance.Adicionar(itemRecompensa);

            aoUsarItemCorreto?.Invoke();
        }
        else
        {
            MensagemUI.Mostrar(textoItemErrado);
        }
    }
}