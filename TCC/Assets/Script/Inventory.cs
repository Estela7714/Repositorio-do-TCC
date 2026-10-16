using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public event Action OnInventarioMudou;
    public event Action OnSelecaoMudou;

    private readonly List<ItemData> itens = new List<ItemData>();
    public IReadOnlyList<ItemData> Itens => itens;
    public ItemData ItemSelecionado { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool Possui(string itemID)
    {
        return itens.Exists(i => i.itemID == itemID);
    }

    public void Adicionar(ItemData item)
    {
        if (item == null || Possui(item.itemID)) return;
        itens.Add(item);
        OnInventarioMudou?.Invoke();
    }

    public void Remover(ItemData item)
    {
        if (item == null) return;
        if (itens.RemoveAll(i => i.itemID == item.itemID) > 0)
        {
            if (ItemSelecionado != null && ItemSelecionado.itemID == item.itemID)
                Selecionar(null);

            OnInventarioMudou?.Invoke();
        }
    }

    // Clicar no mesmo item de novo desmarca
    public void Selecionar(ItemData item)
    {
        ItemSelecionado = (item != null && ItemSelecionado == item) ? null : item;
        OnSelecaoMudou?.Invoke();
    }
}