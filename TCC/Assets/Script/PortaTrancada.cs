using UnityEngine;
using UnityEngine.Events;

public class PortaTrancada : MonoBehaviour
{
    [Header("Estado da Porta")]
    public bool estaTrancada = true;
    [Tooltip("ID exato do item necessário (o mesmo campo 'itemID' do ScriptableObject ItemData)")]
    public string itemNecessarioID = "chave_mansao";
    [Tooltip("Se marcado, o jogador precisa clicar no item no inventário antes de clicar na porta")]
    public bool exigirItemSelecionado = false;

    [Header("Mensagens")]
    [TextArea] public string mensagemTrancada = "Está trancada. Preciso da chave certa.";
    [TextArea] public string mensagemDestrancou = "Você destrancou a passagem!";

    [Header("Ações ao Destrancar")]
    public UnityEvent onDestrancar;

    private void OnMouseDown()
    {
        if (DialogoManager.Aberto) return;
        if (MensagemUI.Aberta) return;

        if (!estaTrancada)
        {
            MensagemUI.Mostrar("A passagem já está destrancada.");
            return;
        }

        if (!string.IsNullOrEmpty(itemNecessarioID) && VerificarItemNoInventario())
        {
            Destrancar();
        }
        else
        {
            MensagemUI.Mostrar(mensagemTrancada);
        }
    }

    public void Destrancar()
    {
        estaTrancada = false;
        MensagemUI.Mostrar(mensagemDestrancou);
        onDestrancar?.Invoke();
    }

    public void TrancarNovamente(string novaMensagem)
    {
        estaTrancada = true;
        if (!string.IsNullOrEmpty(novaMensagem))
            mensagemTrancada = novaMensagem;
    }

    private bool VerificarItemNoInventario()
    {
        if (Inventory.Instance == null) return false;

        if (exigirItemSelecionado)
        {
            return Inventory.Instance.ItemSelecionado != null &&
                   Inventory.Instance.ItemSelecionado.itemID == itemNecessarioID;
        }

        return Inventory.Instance.Possui(itemNecessarioID);
    }
}