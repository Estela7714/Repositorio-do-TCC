using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RelogioPenduloPuzzle : MonoBehaviour
{
    [Header("Itens (Arraste os ScriptableObjects ItemData)")]
    public ItemData itemPonteiro;
    public ItemData itemLenteVermelha;

    [Header("Sprites do Relógio na Sala")]
    [Tooltip("SpriteRenderer do Relógio na cena")]
    public SpriteRenderer spriteRendererRelogio;
    public Sprite spriteFechado;
    public Sprite spriteAberto;

    [Header("Painel de Close-up do Mostrador (UI)")]
    public GameObject painelCloseUp;
    public Image imagemMostradorUI;
    public Sprite spriteMostradorCloseUp;

    [Header("Estado")]
    public bool gavetaAberta = false;

    [Header("Evento Opcional")]
    public UnityEvent onGavetaAberta;

    private void Start()
    {
        if (painelCloseUp != null)
            painelCloseUp.SetActive(false);

        if (spriteRendererRelogio != null && spriteFechado != null)
            spriteRendererRelogio.sprite = spriteFechado;
    }

    // Ligue este método no 'Ao Interagir ()' do ObjetoInteragivel do Relógio
    public void InteragirComRelogio()
    {
        if (gavetaAberta)
        {
            AbrirCloseUp("O relógio permanece parado às 03:15. A porta está aberta e a gaveta secreta está vazia.");
            return;
        }

        if (Inventory.Instance != null && itemPonteiro != null && Inventory.Instance.Possui(itemPonteiro.itemID))
        {
            gavetaAberta = true;
            Inventory.Instance.Remover(itemPonteiro);

            if (itemLenteVermelha != null)
                Inventory.Instance.Adicionar(itemLenteVermelha);

            if (spriteRendererRelogio != null && spriteAberto != null)
                spriteRendererRelogio.sprite = spriteAberto;

            AbrirCloseUp("Você encaixa o Ponteiro de Relógio no mostrador parado às 03:15. A porta se abre e você pega a Lente de Vidro Vermelho!");
            onGavetaAberta?.Invoke();
        }
        else
        {
            AbrirCloseUp("Um antigo relógio de pêndulo, parado exatamente às 03:15. Falta um dos ponteiros e há uma pequena gaveta trancada na base.");
        }
    }

    private void AbrirCloseUp(string mensagem)
    {
        if (painelCloseUp != null)
        {
            if (imagemMostradorUI != null && spriteMostradorCloseUp != null)
                imagemMostradorUI.sprite = spriteMostradorCloseUp;

            painelCloseUp.SetActive(true);
        }

        MensagemUI.Mostrar(mensagem);
    }

    public void FecharCloseUp()
    {
        if (painelCloseUp != null)
            painelCloseUp.SetActive(false);
    }
}