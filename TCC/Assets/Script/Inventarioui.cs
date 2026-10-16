using TMPro;
using UnityEngine;

public class InventarioUI : MonoBehaviour
{
    [SerializeField] private Transform container;              // objeto com Horizontal/Grid Layout Group
    [SerializeField] private SlotInventarioUI slotPrefab;
    [SerializeField] private TextMeshProUGUI textoDescricao;   // opcional: mostra nome e descrição do item selecionado

    private void Start()
    {
        Inventory.Instance.OnInventarioMudou += Atualizar;
        Inventory.Instance.OnSelecaoMudou += Atualizar;
        Atualizar();
    }

    private void OnDestroy()
    {
        if (Inventory.Instance == null) return;
        Inventory.Instance.OnInventarioMudou -= Atualizar;
        Inventory.Instance.OnSelecaoMudou -= Atualizar;
    }

    private void Atualizar()
    {
        foreach (Transform filho in container) Destroy(filho.gameObject);

        ItemData selecionado = Inventory.Instance.ItemSelecionado;

        foreach (ItemData item in Inventory.Instance.Itens)
        {
            SlotInventarioUI slot = Instantiate(slotPrefab, container);
            slot.Configurar(item, item == selecionado, Inventory.Instance.Selecionar);
        }

        if (textoDescricao != null)
        {
            textoDescricao.text = selecionado != null
                ? $"<b>{selecionado.nomeItem}</b>\n{selecionado.descricao}"
                : "";
        }
    }
}