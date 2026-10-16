using System;
using UnityEngine;
using UnityEngine.UI;

// Vai no prefab de slot. Estrutura: Slot (Button) > Icone (Image) e Destaque (Image)
public class SlotInventarioUI : MonoBehaviour
{
    [SerializeField] private Image icone;
    [SerializeField] private GameObject destaque; // moldura exibida quando selecionado
    [SerializeField] private Button botao;

    public void Configurar(ItemData item, bool selecionado, Action<ItemData> aoClicar)
    {
        icone.sprite = item.icone;
        destaque.SetActive(selecionado);
        botao.onClick.RemoveAllListeners();
        botao.onClick.AddListener(() => aoClicar(item));
    }
}