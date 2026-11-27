using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EventoAticadorLareira : MonoBehaviour
{
    public static EventoAticadorLareira Instance { get; private set; }

    [Header("Referência do Atiçador (na Sala da Lareira)")]
    [Tooltip("Arraste o objeto do Atiçador que possui o componente ObjetoInteragivel")]
    public ObjetoInteragivel aticadorInteragivel;

    [Header("Consumir Item e Imagem Rápida")]
    [Tooltip("Arraste o ItemData do Atiçador para ser removido do inventário ao concluir")]
    public ItemData itemAticador;
    [Tooltip("Arraste um objeto Image do Canvas que ficará desativado até a coleta")]
    public GameObject imagemRapidaUI;
    [Tooltip("Tempo em segundos que a imagem ficará visível na tela")]
    public float duracaoImagem = 2f;

    [Header("Teleporte ao Falhar (Voltar para Frente da Mansão)")]
    [Tooltip("ID exato da sala da frente/fora da mansão no RoomManager")]
    public string idSalaFrenteMansao = "mansao_porta_frente";
    [Tooltip("Key do SpawnPoint onde o jogador vai aparecer na frente da mansão")]
    public string spawnKeyFrenteMansao = "vindo_portao";

    [Header("Configuração de Tempo")]
    public float tempoLimite = 25f;
    public TextMeshProUGUI textoCronometro;

    [Header("Mensagens")]
    [TextArea] public string mensagemInicio = "Ouvi um estalo vindo da lareira! Preciso ir rápido antes que trave!";
    [TextArea] public string mensagemFalha = "Demorei demais! Uma força estranha me expulsou para a frente da mansão e perdi meus itens...";

    [Header("Eventos Extras")]
    public UnityEvent onEventoIniciado;
    public UnityEvent onEventoConcluido;
    public UnityEvent onEventoFalhou;

    private float tempoRestante = 0f;
    private bool eventoAtivo = false;
    private bool eventoConcluido = false;

    public bool EventoConcluido => eventoConcluido;
    public bool EventoAtivo => eventoAtivo;

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
        {
            textoCronometro.text = "";
            textoCronometro.gameObject.SetActive(false);
        }

        if (imagemRapidaUI != null)
            imagemRapidaUI.SetActive(false);

        if (aticadorInteragivel != null)
            aticadorInteragivel.DefinirTipo(TipoInteracao.Examinar);
    }

    private void Update()
    {
        if (!eventoAtivo || eventoConcluido) return;

        // Pausa o cronômetro enquanto o jogador lê diálogos ou mensagens na tela
        if (DialogoManager.Aberto || MensagemUI.Aberta) return;

        tempoRestante -= Time.deltaTime;
        AtualizarTextoCronometro();

        if (tempoRestante <= 0f)
        {
            FalharEvento();
        }
    }

    private void AtualizarTextoCronometro()
    {
        if (textoCronometro != null)
            textoCronometro.text = $"Tempo: {Mathf.Max(0f, tempoRestante):F1}s";
    }

    public void IniciarEvento()
    {
        if (eventoConcluido) return;

        if (eventoAtivo)
        {
            MensagemUI.Mostrar("O tempo já está correndo! Vá rápido até a lareira!");
            return;
        }

        eventoAtivo = true;
        tempoRestante = tempoLimite;

        if (aticadorInteragivel != null)
            aticadorInteragivel.DefinirTipo(TipoInteracao.Coletar);

        if (textoCronometro != null)
        {
            AtualizarTextoCronometro(); // Define "Tempo: 25.0s" imediatamente, evitando "New Text"
            textoCronometro.gameObject.SetActive(true);
        }

        if (!string.IsNullOrEmpty(mensagemInicio))
            MensagemUI.Mostrar(mensagemInicio);

        onEventoIniciado?.Invoke();
    }

    public void ConcluirEvento()
    {
        if (!eventoAtivo || eventoConcluido) return;

        eventoAtivo = false;
        eventoConcluido = true;

        if (textoCronometro != null)
            textoCronometro.gameObject.SetActive(false);

        if (itemAticador != null && Inventory.Instance != null)
        {
            Inventory.Instance.Remover(itemAticador);
        }

        if (imagemRapidaUI != null)
        {
            StartCoroutine(ExibirImagemTemporaria());
        }

        onEventoConcluido?.Invoke();
    }

    private IEnumerator ExibirImagemTemporaria()
    {
        imagemRapidaUI.SetActive(true);
        yield return new WaitForSeconds(duracaoImagem);
        imagemRapidaUI.SetActive(false);
    }

    private void FalharEvento()
    {
        eventoAtivo = false;
        tempoRestante = 0f;

        // Remove todos os itens do inventário
        if (Inventory.Instance != null)
        {
            Inventory.Instance.LimparTudo();
        }

        // Reseta o atiçador para Examinar ("Isso pode ser útil...")
        if (aticadorInteragivel != null)
            aticadorInteragivel.DefinirTipo(TipoInteracao.Examinar);

        if (textoCronometro != null)
            textoCronometro.gameObject.SetActive(false);

        // Manda o jogador de volta para a frente da mansão
        if (RoomManager.Instance != null && !string.IsNullOrEmpty(idSalaFrenteMansao))
        {
            RoomManager.Instance.LoadRoom(idSalaFrenteMansao, spawnKeyFrenteMansao);
        }

        if (!string.IsNullOrEmpty(mensagemFalha))
            MensagemUI.Mostrar(mensagemFalha);

        onEventoFalhou?.Invoke();
    }
}