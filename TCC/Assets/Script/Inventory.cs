using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("Receitas de Combinação")]
    [SerializeField] private List<ReceitaItem> receitas = new List<ReceitaItem>();

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
        itens.Insert(0, item); // Insere no início (mais recente primeiro)
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

    public void LimparTudo()
    {
        if (itens.Count == 0 && ItemSelecionado == null) return;

        itens.Clear();
        ItemSelecionado = null;

        OnSelecaoMudou?.Invoke();
        OnInventarioMudou?.Invoke();
    }

    // Clicar no mesmo item desmarca; clicar em outro tenta combinar primeiro
    public void Selecionar(ItemData item)
    {
        // 1. Desmarcar (botão direito ou clique no mesmo item)
        if (item == null || ItemSelecionado == item)
        {
            ItemSelecionado = null;
            OnSelecaoMudou?.Invoke();
            return;
        }

        // 2. Se já havia um item selecionado, tenta combinar com o novo item clicado
        if (ItemSelecionado != null)
        {
            if (TentarCombinar(ItemSelecionado, item))
            {
                return; // Combinação feita com sucesso!
            }
        }

        // 3. Se não havia item selecionado ou não existe receita entre eles, apenas seleciona o novo
        ItemSelecionado = item;
        OnSelecaoMudou?.Invoke();
    }

    private bool TentarCombinar(ItemData item1, ItemData item2)
    {
        foreach (ReceitaItem receita in receitas)
        {
            if (receita != null && receita.CombinaCom(item1, item2))
            {
                // Limpa a seleção antes de modificar a lista
                ItemSelecionado = null;

                // Remove os ingredientes marcados para consumo
                if (receita.consumirItemA)
                    itens.RemoveAll(i => i.itemID == receita.itemA.itemID);

                if (receita.consumirItemB)
                    itens.RemoveAll(i => i.itemID == receita.itemB.itemID);

                // Adiciona o novo item criado (e já deixa ele selecionado para facilitar o uso)
                if (receita.itemResultado != null && !Possui(receita.itemResultado.itemID))
                {
                    itens.Insert(0, receita.itemResultado); // Item criado fica em primeiro
                    ItemSelecionado = receita.itemResultado;
                }

                // Exibe feedback na sua MensagemUI
                if (!string.IsNullOrWhiteSpace(receita.mensagemSucesso))
                    MensagemUI.Mostrar(receita.mensagemSucesso);

                // Atualiza a UI (InventarioUI já escuta esses eventos!)
                OnInventarioMudou?.Invoke();
                OnSelecaoMudou?.Invoke();
                return true;
            }
        }

        return false;
    }
}