using UnityEngine;

[CreateAssetMenu(fileName = "NovaReceita", menuName = "RPG/Receita de Item")]
public class ReceitaItem : ScriptableObject
{
    [Header("Ingredientes (A ordem não importa)")]
    public ItemData itemA;
    public ItemData itemB;

    [Header("Resultado")]
    public ItemData itemResultado;
    [TextArea(2, 4)] public string mensagemSucesso = "Combinei os itens!";

    [Header("Opções")]
    public bool consumirItemA = true;
    public bool consumirItemB = true;

    // Verifica se os dois itens clicados batem com esta receita em qualquer ordem
    public bool CombinaCom(ItemData primeiro, ItemData segundo)
    {
        if (primeiro == null || segundo == null || itemA == null || itemB == null)
            return false;

        bool ordemDireta = (primeiro.itemID == itemA.itemID && segundo.itemID == itemB.itemID);
        bool ordemInversa = (primeiro.itemID == itemB.itemID && segundo.itemID == itemA.itemID);

        return ordemDireta || ordemInversa;
    }
}