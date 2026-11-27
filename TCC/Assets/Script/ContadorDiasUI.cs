using UnityEngine;
using TMPro;

public class ContadorDiasUI : MonoBehaviour
{
    [Header("Referência de Texto")]
    [SerializeField] private TextMeshProUGUI textoDia;

    [Header("Textos por Etapa")]
    [Tooltip("Índice 1 = Dia 1, Índice 2 = Dia 2, etc.")]
    [SerializeField]
    private string[] nomesEtapas = new string[]
    {
        "",
        "Dia 1 - Noite",
        "Dia 2 - Tarde",
        "Dia 2 - Noite"
    };

    private int ultimaEtapa = -1;

    private void Awake()
    {
        if (textoDia == null)
            textoDia = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        AtualizarTexto();
    }

    private void Update()
    {
        if (GameStateManager.Instance == null) return;

        if (GameStateManager.Instance.etapaDoDia != ultimaEtapa)
        {
            AtualizarTexto();
        }
    }

    public void AtualizarTexto()
    {
        if (textoDia == null || GameStateManager.Instance == null) return;

        int etapa = GameStateManager.Instance.etapaDoDia;
        ultimaEtapa = etapa;

        if (nomesEtapas != null && etapa >= 0 && etapa < nomesEtapas.Length && !string.IsNullOrEmpty(nomesEtapas[etapa]))
        {
            textoDia.text = nomesEtapas[etapa];
        }
        else
        {
            textoDia.text = "Dia " + etapa;
        }
    }
}