using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Configurações de Cena")]
    [SerializeField] private string nomeDaCenaGameplay = "Gameplay";

    // Método para iniciar o jogo
    public void IniciarJogo()
    {
        SceneManager.LoadScene(nomeDaCenaGameplay);
    }

    // --- LÓGICA DO PANEL ---

    // Ativa um Painel (ex: abrir menu de configurações/opções)
    public void AbrirPainel(GameObject painel)
    {
        if (painel != null)
        {
            painel.SetActive(true);
        }
    }

    // Desativa um Painel (ex: fechar menu/voltar)
    public void FecharPainel(GameObject painel)
    {
        if (painel != null)
        {
            painel.SetActive(false);
        }
    }

    // Alterna o estado do Painel (se estiver ativo, desativa; se desativo, ativa)
    public void AlternarPainel(GameObject painel)
    {
        if (painel != null)
        {
            painel.SetActive(!painel.activeSelf);
        }
    }
}
