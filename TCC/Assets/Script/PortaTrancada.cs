using UnityEngine;
using UnityEngine.Events;

public class PortaTrancada : MonoBehaviour
{
    [Header("Estado da Porta")]
    public bool estaTrancada = true;
    [Tooltip("ID exato do item necessário (deixe vazio se a porta abrir por enigma/cadeado)")]
    public string itemNecessarioID = "chave_mansao";
    [Tooltip("Se marcado, o jogador precisa clicar no item no inventário antes de clicar na porta")]
    public bool exigirItemSelecionado = false;

    [Header("Mensagens")]
    [TextArea] public string mensagemTrancada = "Está trancada.";
    [TextArea] public string mensagemDestrancou = "Você destrancou a passagem!";

    [Header("Ações")]
    public UnityEvent onInteragirTrancada;
    public UnityEvent onDestrancar;
    [Tooltip("Executado sempre que clicar na porta depois que ela já estiver destrancada")]
    public UnityEvent onInteragirDestrancada;

    private void OnMouseDown()
    {
        if (DialogoManager.Aberto) return;
        if (MensagemUI.Aberta) return;

        if (!estaTrancada)
        {
            onInteragirDestrancada?.Invoke();
            return;
        }

        if (!string.IsNullOrEmpty(itemNecessarioID) && VerificarItemNoInventario())
        {
            Destrancar();
        }
        else
        {
            if (!string.IsNullOrEmpty(mensagemTrancada))
                MensagemUI.Mostrar(mensagemTrancada);

            onInteragirTrancada?.Invoke();
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