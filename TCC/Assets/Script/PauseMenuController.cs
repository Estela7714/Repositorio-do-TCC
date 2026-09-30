using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Painéis de UI")]
    [SerializeField] private GameObject painelPause;
    [SerializeField] private GameObject painelConfirmacao;

    [Header("Configurações de Cenas")]
    [SerializeField] private string nomeDaCenaMenuPrincipal = "MenuPrincipal";

    private bool jogoPausado = false;

    void Update()
    {
        // Tecla ESC para abrir/fechar o pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
            {
                // Se o aviso de confirmação estiver aberto ao apertar ESC, apenas fecha a confirmação
                if (painelConfirmacao != null && painelConfirmacao.activeSelf)
                {
                    CancelarSaida();
                }
                else
                {
                    ContinuarJogo();
                }
            }
            else
            {
                PausarJogo();
            }
        }
    }

    // --- LÓGICA DE PAUSE ---

    public void ContinuarJogo()
    {
        painelPause.SetActive(false);
        if (painelConfirmacao != null) painelConfirmacao.SetActive(false);

        Time.timeScale = 1f; // Volta o tempo do jogo ao normal
        jogoPausado = false;
    }

    public void PausarJogo()
    {
        painelPause.SetActive(true);
        if (painelConfirmacao != null) painelConfirmacao.SetActive(false);

        Time.timeScale = 0f; // Congela o jogo
        jogoPausado = true;
    }

    // --- LÓGICA DE CONFIRMAÇÃO ---

    // Chame este método no botão "Voltar ao Menu" do Painel de Pause
    public void SolicitarConfirmacaoSaida()
    {
        if (painelConfirmacao != null)
        {
            painelConfirmacao.SetActive(true); // Exibe o aviso "Tem certeza?"
        }
    }

    // Chame este método no botão "NÃO" (ou "Cancelar") do painel de aviso
    public void CancelarSaida()
    {
        if (painelConfirmacao != null)
        {
            painelConfirmacao.SetActive(false); // Esconde o aviso e mantém o menu de pause visível
        }
    }

    // Chame este método no botão "SIM" do painel de aviso
    public void ConfirmarVoltarAoMenu()
    {
        Time.timeScale = 1f; // RESTAURA O TEMPO ANTES DE MUDAR DE CENA
        SceneManager.LoadScene(nomeDaCenaMenuPrincipal);
    }

    // Opcional: Se quiser recarregar a fase atual
    public void ConfirmarReiniciarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}