using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EventoAticadorLareira : MonoBehaviour
{
    public static EventoAticadorLareira Instance { get; private set; }

    [Header("Configuração de Tempo")]
    [Tooltip("Tempo em segundos para ir da Sala de Quadros até a Lareira")]
    public float tempoLimite = 15f;

    [Header("UI do Cronômetro (Opcional)")]
    [Tooltip("Arraste um TextMeshProUGUI do Canvas para mostrar a contagem regressiva")]
    public TextMeshProUGUI textoCronometro;

    [Header("Mensagens")]
    [TextArea] public string mensagemInicio = "Ouvi um estalo vindo da lareira! Preciso ir rápido antes que feche!";
    [TextArea] public string mensagemFalha = "Demorei demais... o mecanismo voltou ao normal.";
    [TextArea] public string mensagemSucesso = "Consegui pegar o atiçador a tempo!";

    [Header("Eventos")]
    public UnityEvent onEventoIniciado;
    public UnityEvent onEventoConcluido;
    public UnityEvent onEventoFalhou;

    private float tempoRestante = 0f;
    private bool eventoAtivo = false;
    private bool eventoConcluido = false;

    public bool EventoAtivo => eventoAtivo;
    public bool EventoConcluido => eventoConcluido;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (textoCronometro != null)
            textoCronometro.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!eventoAtivo || eventoConcluido) return;

        // Pausa a contagem enquanto houver diálogo aberto na tela (opcional)
        if (DialogoManager.Aberto) return;

        tempoRestante -= Time.deltaTime;

        if (textoCronometro != null)
        {
            textoCronometro.text = $"Tempo: {Mathf.Max(0f, tempoRestante):F1}s";
        }

        if (tempoRestante <= 0f)
        {
            FalharEvento();
        }
    }

    // Chame este método ao interagir com o Quadro
    public void IniciarEvento()
    {
        if (eventoConcluido) return;

        if (eventoAtivo)
        {
            MensagemUI.Mostrar("O tempo já está correndo! Vá até a lareira!");
            return;
        }

        eventoAtivo = true;
        tempoRestante = tempoLimite;

        if (textoCronometro != null)
            textoCronometro.gameObject.SetActive(true);

        if (!string.IsNullOrEmpty(mensagemInicio))
            MensagemUI.Mostrar(mensagemInicio);

        onEventoIniciado?.Invoke();
    }

    // Chame este método ao clicar/interagir com o Atiçador na Lareira
    public bool TentarColetarAticador()
    {
        if (eventoConcluido) return false;

        if (!eventoAtivo)
        {
            MensagemUI.Mostrar("O atiçador parece preso no suporte da lareira.");
            return false;
        }

        eventoAtivo = false;
        eventoConcluido = true;

        if (textoCronometro != null)
            textoCronometro.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(mensagemSucesso))
            MensagemUI.Mostrar(mensagemSucesso);

        onEventoConcluido?.Invoke();
        return true;
    }

    private void FalharEvento()
    {
        eventoAtivo = false;
        tempoRestante = 0f;

        if (textoCronometro != null)
            textoCronometro.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(mensagemFalha))
            MensagemUI.Mostrar(mensagemFalha);

        onEventoFalhou?.Invoke();
    }
}