using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventarioUI : MonoBehaviour
{
    public static InventarioUI Instance { get; private set; }

    [Header("Controle de Abertura")]
    [SerializeField] private GameObject painelInventario;        // O painel que abre e fecha
    [SerializeField] private Button botaoAbrirInventario;        // Botão na tela para abrir/fechar
    [SerializeField] private Button botaoFecharInventario;       // Botão "X" dentro do painel (opcional)

    [Header("Slots e Descrição")]
    [SerializeField] private Transform container;                // Objeto com Horizontal/Grid Layout Group
    [SerializeField] private SlotInventarioUI slotPrefab;
    [SerializeField] private TextMeshProUGUI textoDescricao;     // Mostra nome e descrição do item selecionado

    public static bool Aberto => Instance != null && Instance.painelInventario != null && Instance.painelInventario.activeSelf;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (painelInventario != null)
            painelInventario.SetActive(false);

        if (botaoAbrirInventario != null)
            botaoAbrirInventario.onClick.AddListener(AlternarInventario);

        if (botaoFecharInventario != null)
            botaoFecharInventario.onClick.AddListener(FecharInventario);
    }

    private void Start()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventarioMudou += Atualizar;
            Inventory.Instance.OnSelecaoMudou += Atualizar;
            Atualizar();
        }
    }

    private void OnDestroy()
    {
        if (Inventory.Instance == null) return;
        Inventory.Instance.OnInventarioMudou -= Atualizar;
        Inventory.Instance.OnSelecaoMudou -= Atualizar;
    }

    public void AlternarInventario()
    {
        if (DialogoManager.Aberto) return; // Evita abrir por cima de um diálogo

        if (painelInventario.activeSelf)
            FecharInventario();
        else
            AbrirInventario();
    }

    public void AbrirInventario()
    {
        if (painelInventario != null)
        {
            painelInventario.SetActive(true);
            Atualizar();
        }
    }

    public void FecharInventario()
    {
        if (painelInventario != null)
            painelInventario.SetActive(false);
    }

    private void Atualizar()
    {
        if (Inventory.Instance == null || container == null || slotPrefab == null) return;

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
                ? $"<b>{selecionado.nomeItem}</b>\n{selecionado.descricao}\n<i>(Clique em outro item para combinar)</i>"
                : "Selecione um item para inspecionar ou combinar.";
        }
    }
}