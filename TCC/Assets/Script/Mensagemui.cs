using TMPro;
using UnityEngine;

// Caixa de texto usada para descrições e informações.
// IMPORTANTE: coloque este script em um objeto que NÃO seja o próprio painel
// (ex: no Canvas), pois o painel é desativado.
public class MensagemUI : MonoBehaviour
{
    public static MensagemUI Instance { get; private set; }

    [SerializeField] private GameObject painel;
    [SerializeField] private TextMeshProUGUI texto;

    public static bool Aberta => Instance != null && Instance.painel != null && Instance.painel.activeSelf;

    private void Awake()
    {
        Instance = this;
        painel.SetActive(false);
    }

    public static void Mostrar(string mensagem)
    {
        if (Instance == null || string.IsNullOrWhiteSpace(mensagem)) return;
        Instance.texto.text = mensagem;
        Instance.painel.SetActive(true);
    }

    public static void Fechar()
    {
        if (Instance != null) Instance.painel.SetActive(false);
    }
}