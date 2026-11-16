using UnityEngine;
using UnityEngine.Events;

public class RouboGuardaDelegacia : MonoBehaviour
{
    [Header("Assets ItemData (Arraste da pasta Project)")]
    [Tooltip("Arraste o item final combinado (ex: Ferramenta de Pesca)")]
    public ItemData itemFerramentaFinal;
    [Tooltip("Arraste o ItemData da Chave da Mansão")]
    public ItemData itemChaveMansao;

    [Header("Estado")]
    public bool chaveRoubada = false;

    [Header("Evento ao Conseguir a Chave")]
    public UnityEvent onChaveRoubada;

    private void OnMouseDown()
    {
        if (DialogoManager.Aberto) return;
        if (MensagemUI.Aberta) return;

        if (chaveRoubada)
        {
            MensagemUI.Mostrar("O guarda continua roncando. Já peguei a Chave da Mansão.");
            return;
        }

        if (Inventory.Instance == null) return;

        // Verifica se o jogador já combinou tudo e possui a ferramenta final no inventário
        if (itemFerramentaFinal != null && Inventory.Instance.Possui(itemFerramentaFinal.itemID))
        {
            chaveRoubada = true;
            Inventory.Instance.Remover(itemFerramentaFinal);

            if (itemChaveMansao != null)
                Inventory.Instance.Adicionar(itemChaveMansao);

            MensagemUI.Mostrar("Com o ímã lubrificado, encerado e preso ao barbante, você pesca a Chave da Mansão silenciosamente sem acordar o guarda!");
            onChaveRoubada?.Invoke();
        }
        else
        {
            MensagemUI.Mostrar("O guarda dorme pesado com o molho de chaves preso ao cinto. Puxar agora faria o metal tilintar e o acordaria. Preciso improvisar uma ferramenta silenciosa no inventário...");
        }
    }
}