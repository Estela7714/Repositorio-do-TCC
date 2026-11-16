using UnityEngine;

public class QuadroReveladorPuzzle : MonoBehaviour
{
    [Header("Configuração da Lente")]
    public ItemData itemLenteVermelha;
    [Tooltip("Se marcado, o jogador precisa clicar na Lente no inventário antes de clicar no quadro")]
    public bool exigirLenteSelecionada = false;

    [Header("Visual Opcional")]
    [Tooltip("Objeto/Sprite com as marcas de sangue ocultas que aparece ao olhar com a lente")]
    public GameObject camadaSangueOculto;

    [Header("Textos")]
    [TextArea] public string textoNormal = "Uma pintura antiga da família do duque. Não parece haver nada de incomum.";
    [TextArea] public string textoRevelado = "Olhando através da Lente de Vidro Vermelho, marcas de sangue brilham na tela: uma LUA, um ENFORCADO e TRÊS GOTAS DE SANGUE!";

    private void Start()
    {
        if (camadaSangueOculto != null)
            camadaSangueOculto.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (DialogoManager.Aberto) return;
        if (MensagemUI.Aberta) return;

        bool usandoLente = false;

        if (Inventory.Instance != null && itemLenteVermelha != null)
        {
            if (exigirLenteSelecionada)
                usandoLente = Inventory.Instance.ItemSelecionado != null &&
                              Inventory.Instance.ItemSelecionado.itemID == itemLenteVermelha.itemID;
            else
                usandoLente = Inventory.Instance.Possui(itemLenteVermelha.itemID);
        }

        if (usandoLente)
        {
            if (camadaSangueOculto != null)
                camadaSangueOculto.SetActive(true);

            MensagemUI.Mostrar(textoRevelado);
        }
        else
        {
            MensagemUI.Mostrar(textoNormal);
        }
    }
}