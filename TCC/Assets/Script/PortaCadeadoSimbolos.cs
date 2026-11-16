using UnityEngine;

public class PortaCadeadoSimbolos : MonoBehaviour
{
    [Header("Referência do Cadeado")]
    [Tooltip("Arraste o objeto que contém o script CadeadoSimbolosUI (ex: Canvas)")]
    public CadeadoSimbolosUI cadeadoUI;

    private void OnMouseDown()
    {
        if (DialogoManager.Aberto) return;
        if (MensagemUI.Aberta) return;

        if (cadeadoUI != null)
        {
            cadeadoUI.AbrirPainel();
        }
        else
        {
            Debug.LogWarning("CadeadoSimbolosUI não foi atribuído no Inspector de " + gameObject.name);
        }
    }
}