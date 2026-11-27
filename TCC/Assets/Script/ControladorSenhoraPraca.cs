using UnityEngine;

public class ControladorSenhoraPraca : MonoBehaviour
{
    [Header("Versões do NPC na Praça")]
    [Tooltip("Senhora com as falas do Dia 1 (lenda) / Tarde do Dia 2.")]
    [SerializeField] private GameObject senhoraDia1;

    [Tooltip("Senhora com a fala que incita o roubo na delegacia (Segunda Noite).")]
    [SerializeField] private GameObject senhoraSegundaNoite;

    [Header("Etapa do Dia para Liberar")]
    [Tooltip("Ex: 3 se 1 = Noite 1, 2 = Tarde Dia 2, 3 = Noite Dia 2.")]
    [SerializeField] private int etapaSegundaNoite = 3;

    private void OnEnable()
    {
        AtualizarVersaoSenhora();
    }

    public void AtualizarVersaoSenhora()
    {
        bool ehSegundaNoite = GameStateManager.Instance != null &&
                              GameStateManager.Instance.etapaDoDia == etapaSegundaNoite;

        if (senhoraDia1 != null)
            senhoraDia1.SetActive(!ehSegundaNoite);

        if (senhoraSegundaNoite != null)
            senhoraSegundaNoite.SetActive(ehSegundaNoite);
    }
}