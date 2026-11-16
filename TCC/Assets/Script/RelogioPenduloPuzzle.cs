using UnityEngine;
using UnityEngine.Events;

public class RelogioPenduloPuzzle : MonoBehaviour
{
    [Header("Itens (Arraste os ScriptableObjects ItemData)")]
    public ItemData itemPonteiro;
    public ItemData itemLenteVermelha;

    [Header("Estado")]
    public bool gavetaAberta = false;

    [Header("Evento Opcional (Ex: Trocar sprite do relógio)")]
    public UnityEvent onGavetaAberta;

    private void OnMouseDown()
    {
        if (DialogoManager.Aberto) return;
        if (MensagemUI.Aberta) return;

        if (gavetaAberta)
        {
            MensagemUI.Mostrar("O relógio permanece parado às 03:15. A gaveta secreta está vazia.");
            return;
        }

        if (Inventory.Instance != null && itemPonteiro != null && Inventory.Instance.Possui(itemPonteiro.itemID))
        {
            gavetaAberta = true;
            Inventory.Instance.Remover(itemPonteiro);

            if (itemLenteVermelha != null)
                Inventory.Instance.Adicionar(itemLenteVermelha);

            MensagemUI.Mostrar("Você encaixa o Ponteiro de Relógio no mostrador parado às 03:15. Um mecanismo estala e abre uma gaveta oculta! Você pegou a Lente de Vidro Vermelho.");
            onGavetaAberta?.Invoke();
        }
        else
        {
            MensagemUI.Mostrar("Um antigo relógio de pêndulo, parado exatamente às 03:15. Falta um dos ponteiros e há uma pequena gaveta trancada na base.");
        }
    }
}