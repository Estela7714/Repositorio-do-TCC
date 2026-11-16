using UnityEngine;

public class CamaInteragivel : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (DialogoManager.Aberto) return;
        if (MensagemUI.Aberta) return;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.TentarDormir();
        }
    }
}