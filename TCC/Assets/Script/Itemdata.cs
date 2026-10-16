using UnityEngine;

// Cada item do jogo é um asset: Botão direito no Project > Create > RPG > Item Data
[CreateAssetMenu(fileName = "NovoItem", menuName = "RPG/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemID;          // Ex: "chave_porao" (único para cada item)
    public string nomeItem;        // Ex: "Chave enferrujada"
    public Sprite icone;
    [TextArea(2, 5)] public string descricao;
}