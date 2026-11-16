using UnityEngine;

public class AtivarClientesMesa : MonoBehaviour
{
    [Tooltip("Arraste o objeto NPC_ClientesMesa para cá")]
    public GameObject npcClientesMesa;

    private void OnEnable()
    {
        VerificarCondicoes();
    }

    public void VerificarCondicoes()
    {
        if (GameStateManager.Instance == null || npcClientesMesa == null) return;

        var gm = GameStateManager.Instance;

        // Só aparece se for a Noite do Dia 1 E as 3 primeiras pistas já tiverem sido obtidas
        bool deveAparecer = (gm.etapaDoDia == 1)
                            && gm.falouComAtendente
                            && gm.ouviuMoradoresBeco
                            && gm.ouviuLendaPraca;

        npcClientesMesa.SetActive(deveAparecer);
    }
}