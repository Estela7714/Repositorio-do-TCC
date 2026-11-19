using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class SlotPedestal
{
    public ItemData itemNecessario;
    public GameObject visualNaCena;
    public GameObject visualNoCloseUp;
    [HideInInspector] public bool colocado = false;
}

public class EstanteSecretaPedestal : MonoBehaviour
{
    [Header("Os 3 Itens do Pedestal")]
    public SlotPedestal[] slots = new SlotPedestal[3];
    public bool consumirItensAoColocar = true;

    [Header("Painel de Close-Up (UI)")]
    public GameObject painelCloseUpPedestal;
    public Image imagemPedestalUI;
    public Sprite spritePedestalVazio;
    public Sprite spritePedestalCompleto;

    [Header("Bloqueio da Estante")]
    [Tooltip("Arraste o objeto Bloqueio_EstanteSecreta da cena")]
    public GameObject bloqueioEstante;

    [Header("Mensagens")]
    [TextArea] public string mensagemFaltamItens = "Um pedestal com três encaixes esculpidos na pedra. Ainda faltam peças para ativar o mecanismo.";
    [TextArea] public string mensagemItemColocado = "Você encaixou uma das peças no pedestal!";
    [TextArea] public string mensagemCompleto = "As três peças se alinham! Um mecanismo range por trás da parede e a estante desliza revelando uma passagem secreta!";
    [TextArea] public string mensagemJaResolvido = "O mecanismo do pedestal já está completo e a passagem está aberta.";

    [Header("Evento ao Abrir a Estante")]
    public UnityEvent onEstanteAberta;

    [Header("Amuletos Liberados Após Examinar o Pedestal")]
    [Tooltip("Arraste os 3 amuletos coletáveis das salas. Eles começam escondidos e só aparecem após clicar no pedestal.")]
    public GameObject[] amuletosParaRevelar;
    private bool amuletosJaRevelados = false;

    public static bool EstanteAberta { get; private set; } = false;

    private void Start()
    {
        if (painelCloseUpPedestal != null)
            painelCloseUpPedestal.SetActive(false);

        if (!amuletosJaRevelados && amuletosParaRevelar != null)
        {
            for (int i = 0; i < amuletosParaRevelar.Length; i++)
            {
                if (amuletosParaRevelar[i] != null)
                    amuletosParaRevelar[i].SetActive(false);
            }
        }

        AtualizarVisuais();
    }

    private void OnEnable()
    {
        // Garante que ao voltar para a sala do escritório o bloqueio continue aberto se já foi resolvido
        if (bloqueioEstante != null)
            bloqueioEstante.SetActive(!EstanteAberta);
    }

    public void InteragirComPedestal()
    {
        if (DialogoManager.Aberto || MensagemUI.Aberta) return;

        // Revela os amuletos nas salas no primeiro clique no pedestal
        if (!amuletosJaRevelados)
        {
            amuletosJaRevelados = true;
            if (amuletosParaRevelar != null)
            {
                for (int i = 0; i < amuletosParaRevelar.Length; i++)
                {
                    if (amuletosParaRevelar[i] != null)
                        amuletosParaRevelar[i].SetActive(true);
                }
            }
        }

        bool encaixouAlgoAgora = false;

        if (!EstanteAberta && Inventory.Instance != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].colocado && slots[i].itemNecessario != null)
                {
                    if (Inventory.Instance.Possui(slots[i].itemNecessario.itemID))
                    {
                        slots[i].colocado = true;
                        encaixouAlgoAgora = true;

                        if (consumirItensAoColocar)
                            Inventory.Instance.Remover(slots[i].itemNecessario);
                    }
                }
            }
        }

        int totalColocados = ContarColocados();

        if (!EstanteAberta && totalColocados >= slots.Length)
        {
            EstanteAberta = true;
            AtualizarVisuais();
            AbrirCloseUp(mensagemCompleto);
            onEstanteAberta?.Invoke();
            return;
        }

        AtualizarVisuais();

        if (EstanteAberta)
        {
            AbrirCloseUp(mensagemJaResolvido);
        }
        else if (encaixouAlgoAgora)
        {
            AbrirCloseUp($"{mensagemItemColocado} ({totalColocados}/{slots.Length})");
        }
        else
        {
            AbrirCloseUp($"{mensagemFaltamItens} ({totalColocados}/{slots.Length})");
        }
    }

    private void AbrirCloseUp(string mensagem)
    {
        if (painelCloseUpPedestal != null)
            painelCloseUpPedestal.SetActive(true);

        if (!string.IsNullOrEmpty(mensagem))
            MensagemUI.Mostrar(mensagem);
    }

    public void FecharCloseUp()
    {
        if (painelCloseUpPedestal != null)
            painelCloseUpPedestal.SetActive(false);
    }

    private int ContarColocados()
    {
        int count = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].colocado) count++;
        }
        return count;
    }

    private void AtualizarVisuais()
    {
        if (bloqueioEstante != null)
            bloqueioEstante.SetActive(!EstanteAberta);

        if (imagemPedestalUI != null)
        {
            if (EstanteAberta && spritePedestalCompleto != null)
                imagemPedestalUI.sprite = spritePedestalCompleto;
            else if (spritePedestalVazio != null)
                imagemPedestalUI.sprite = spritePedestalVazio;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].visualNaCena != null)
                slots[i].visualNaCena.SetActive(slots[i].colocado);

            if (slots[i].visualNoCloseUp != null)
                slots[i].visualNoCloseUp.SetActive(slots[i].colocado);
        }
    }
}