using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDeJogoSalaSecreta : MonoBehaviour
{
    [Header("Painel de Fim / Continua")]
    [Tooltip("Arraste o Painel do Canvas escrito 'Continua'")]
    [SerializeField] private GameObject painelContinua;

    [Header("Configuração de Tempo e Cena")]
    [Tooltip("Tempo em segundos que a tela 'Continua' fica visível antes de voltar ao menu")]
    [SerializeField] private float tempoExibicao = 4f;

    [Tooltip("Nome ou índice (0) da primeira cena do jogo (tela de Play)")]
    [SerializeField] private string nomeCenaMenu = "";
    [SerializeField] private int indiceCenaMenu = 0;

    private bool disparado = false;

    private void Awake()
    {
        if (painelContinua != null)
            painelContinua.SetActive(false);
    }

    private void OnEnable()
    {
        AtivarFinal();
    }

    // Chame este método quando o personagem entrar na sala secreta
    public void AtivarFinal()
    {
        if (disparado) return;
        disparado = true;

        StartCoroutine(SequenciaFimDeJogo());
    }

    private IEnumerator SequenciaFimDeJogo()
    {
        // Trava o movimento do jogador para ele não sair andando no fundo
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            player.StopMovement();
            player.enabled = false;
        }

        // Garante que o painel fique na frente de toda a UI (inventário, mensagens, etc.)
        if (painelContinua != null)
        {
            painelContinua.transform.SetAsLastSibling();
            painelContinua.SetActive(true);
        }

        // Usa tempo real (funciona mesmo se Time.timeScale estiver 0)
        yield return new WaitForSecondsRealtime(tempoExibicao);

        Time.timeScale = 1f;

        // Carrega a primeira cena do jogo (tela de Play)
        if (!string.IsNullOrWhiteSpace(nomeCenaMenu))
            SceneManager.LoadScene(nomeCenaMenu);
        else
            SceneManager.LoadScene(indiceCenaMenu);
    }
}