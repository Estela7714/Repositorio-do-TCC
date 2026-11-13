using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class NPCInteragivel : MonoBehaviour
{
    [Header("Conversa")]
    [SerializeField] private LinhaDialogo[] falas;
    [SerializeField] private UnityEvent aoTerminarConversa;

    [Header("Depois que o NPC já falou tudo")]
    [TextArea(2, 4)]
    [SerializeField] private string mensagemSemMaisNada = "Parece que eu não vou conseguir mais nada.";

    private bool jaFalouTudo = false;

    public void Conversar()
    {
        // Se já terminou o diálogo antes, mostra apenas a mensagem rápida na MensagemUI
        if (jaFalouTudo)
        {
            MensagemUI.Mostrar(mensagemSemMaisNada);
            return;
        }

        // Inicia a sequência de falas no DialogoManager
        DialogoManager.Instance.IniciarDialogo(falas, () =>
        {
            jaFalouTudo = true;
            aoTerminarConversa?.Invoke();
        });
    }
}