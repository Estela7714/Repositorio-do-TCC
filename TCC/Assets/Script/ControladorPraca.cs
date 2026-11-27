using UnityEngine;

public class ControladorPraca : MonoBehaviour
{
    [Header("NPCs da Praça")]
    public GameObject npcDia1;
    public GameObject npcNoite2;

    private void OnEnable()
    {
        AtualizarNPCs();
    }

    public void AtualizarNPCs()
    {
        if (GameStateManager.Instance == null) return;

        int etapa = GameStateManager.Instance.etapaDoDia;
        bool viuPortao = GameStateManager.Instance.visitouPortaoMansao;

        // 1. Etapa 1 (Noite Dia 1): mostra apenas a primeira Senhora
        if (etapa == 1)
        {
            if (npcDia1 != null) npcDia1.SetActive(true);
            if (npcNoite2 != null) npcNoite2.SetActive(false);
        }
        // 2. Etapa 2 (Tarde Dia 2) + já visitou o portão: mostra apenas a segunda Senhora
        else if (etapa == 2 && viuPortao)
        {
            if (npcDia1 != null) npcDia1.SetActive(false);
            if (npcNoite2 != null) npcNoite2.SetActive(true);
        }
        // 3. Qualquer outro momento (Tarde Dia 2 antes do portão, Noite 2, Dia 3): ambas somem
        else
        {
            if (npcDia1 != null) npcDia1.SetActive(false);
            if (npcNoite2 != null) npcNoite2.SetActive(false);
        }
    }
}