using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("Controle de Tempo")]
    [Tooltip("1 = Noite Dia 1 | 2 = Dia 2 (15h/Tarde) | 3 = Noite Dia 2 (Delegacia/Mansão) | 4 = Amanhecer Dia 3")]
    public int etapaDoDia = 1;

    [Header("Pistas do Dia 1 (Noite)")]
    public bool falouComAtendente = false;
    public bool ouviuMoradoresBeco = false;
    public bool ouviuLendaPraca = false;
    public bool ouviuConversaMesaSalao = false; // Nova pista do fim do Dia 1

    [Header("Pistas do Dia 2 (15:00 e Tarde)")]
    public bool visitouPortaoMansao = false;
    public bool falouComSenhoraTarde = false;

    [Header("Eventos ao Mudar de Etapa")]
    public UnityEvent onMudarEtapa;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // --- Métodos chamados pelos NPCs / Objetos ---
    public void MarcarAtendente() => falouComAtendente = true;
    public void MarcarBeco() => ouviuMoradoresBeco = true;
    public void MarcarLendaPraca() => ouviuLendaPraca = true;
    public void MarcarConversaMesa() => ouviuConversaMesaSalao = true;
    public void MarcarPortaoMansao() => visitouPortaoMansao = true;
    public void MarcarSenhoraTarde() => falouComSenhoraTarde = true;

    // --- Método chamado ao clicar na Cama da Pousada ---
    public void TentarDormir()
    {
        if (etapaDoDia == 1)
        {
            if (falouComAtendente && ouviuMoradoresBeco && ouviuLendaPraca && ouviuConversaMesaSalao)
            {
                AvancarEtapa("Você janta e vai descansar até a tarde seguinte...");
            }
            else
            {
                MostrarAviso("Ainda não descobri o suficiente sobre a cidade para ir dormir.");
            }
        }
        else if (etapaDoDia == 2)
        {
            if (visitouPortaoMansao && falouComSenhoraTarde)
            {
                AvancarEtapa("Você aguarda no quarto até o cair da noite para invadir a delegacia...");
            }
            else
            {
                MostrarAviso("Ainda preciso investigar o portão da mansão e falar com a senhora antes de anoitecer.");
            }
        }
        else if (etapaDoDia == 3)
        {
            MostrarAviso("Não há tempo para dormir agora. Preciso ir até a delegacia pegar a chave!");
        }
        else if (etapaDoDia == 4)
        {
            AvancarEtapa("Você desaba na cama e dorme até a hora do jantar, aguardando o amanhecer para voltar para casa...");
        }
    }

    private void AvancarEtapa(string mensagemTransicao)
    {
        etapaDoDia++;
        MostrarAviso(mensagemTransicao);
        onMudarEtapa?.Invoke();
    }

    private void MostrarAviso(string texto)
    {
        MensagemUI.Mostrar(texto);
    }
}