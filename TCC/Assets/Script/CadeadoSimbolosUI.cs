using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CadeadoSimbolosUI : MonoBehaviour
{
    [Header("Referências de UI")]
    public GameObject painelCadeado;
    public TextMeshProUGUI[] textosDiscos; // Arraste os 3 textos dos botões (Slot 0, 1 e 2)

    [Header("Configuração dos Símbolos")]
    [Tooltip("Lista de opções que giram em cada disco")]
    public string[] simbolosDisponiveis = { "Sol", "Lua", "Corvo", "Enforcado", "Crânio", "3 Gotas de Sangue", "Espiral" };

    [Header("Senha Correta (Índices da lista acima)")]
    [Tooltip("Por padrão: 1 = Lua | 3 = Enforcado | 5 = 3 Gotas de Sangue")]
    public int[] senhaCorreta = { 1, 3, 5 };

    [Header("Evento ao Destrancar a Porta")]
    public UnityEvent onCadeadoAberto;

    private int[] indicesAtuais = { 0, 0, 0 };
    private bool resolvido = false;

    private void Start()
    {
        if (painelCadeado != null)
            painelCadeado.SetActive(false);

        AtualizarTextos();
    }

    // Chame este método ao clicar na porta trancada do Quarto do Dono
    public void AbrirPainel()
    {
        if (DialogoManager.Aberto || MensagemUI.Aberta) return;

        if (resolvido)
        {
            MensagemUI.Mostrar("O cadeado místico já está aberto.");
            return;
        }

        if (painelCadeado != null)
        {
            painelCadeado.SetActive(true);
            AtualizarTextos();
        }
    }

    public void FecharPainel()
    {
        if (painelCadeado != null)
            painelCadeado.SetActive(false);
    }

    // Ligue nos 3 botões do painel (passando 0, 1 ou 2 no OnClick)
    public void ProximoSimbolo(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= indicesAtuais.Length) return;

        indicesAtuais[slotIndex] = (indicesAtuais[slotIndex] + 1) % simbolosDisponiveis.Length;
        AtualizarTextos();
    }

    // Ligue no botão "Confirmar / Abrir"
    public void VerificarCombinacao()
    {
        if (indicesAtuais[0] == senhaCorreta[0] &&
            indicesAtuais[1] == senhaCorreta[1] &&
            indicesAtuais[2] == senhaCorreta[2])
        {
            resolvido = true;
            FecharPainel();
            MensagemUI.Mostrar("Os símbolos brilham em vermelho e o cadeado místico se desfaz! A porta para o Quarto do Ocultista está aberta.");
            onCadeadoAberto?.Invoke();
        }
        else
        {
            FecharPainel();
            MensagemUI.Mostrar("Nada acontece. A charada diz: 'O tempo para quando o nó se aperta; a hora da morte dita o segredo.'");
        }
    }

    private void AtualizarTextos()
    {
        if (textosDiscos == null) return;

        for (int i = 0; i < textosDiscos.Length && i < indicesAtuais.Length; i++)
        {
            if (textosDiscos[i] != null)
                textosDiscos[i].text = simbolosDisponiveis[indicesAtuais[i]];
        }
    }
}