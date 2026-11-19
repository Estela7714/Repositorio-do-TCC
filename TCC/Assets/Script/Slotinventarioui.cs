using System;
using UnityEngine;
using UnityEngine.UI;

public class SlotInventarioUI : MonoBehaviour
{
    [SerializeField] private Image icone;
    [SerializeField] private GameObject destaque; // Pode deixar None no Inspector
    [SerializeField] private Button botao;

    private static readonly Color CorNormal = Color.white;
    private static readonly Color CorSelecionado = new Color(0.55f, 0.55f, 0.55f, 1f); // Fica mais escuro quando clicado

    public void Configurar(ItemData item, bool selecionado, Action<ItemData> aoClicar)
    {
        icone.sprite = item.icone;
        icone.color = selecionado ? CorSelecionado : CorNormal;

        if (destaque != null)
            destaque.SetActive(false);

        botao.onClick.RemoveAllListeners();
        botao.onClick.AddListener(() => aoClicar(item));
    }
}