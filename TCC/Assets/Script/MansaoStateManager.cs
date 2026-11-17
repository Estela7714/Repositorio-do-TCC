using UnityEngine;
using System;

public class MansaoStateManager : MonoBehaviour
{
    public static MansaoStateManager Instance { get; private set; }

    [Header("Estado da Mansão")]
    public bool bossDerrotado = false;

    [Header("Configuração da Porta Principal Trancada")]
    public string idSalaSalao = "mansao_salao";
    public float posicaoXPortaPrincipal = 6.5f;
    public float alcanceBloqueio = 1.6f;
    [TextArea]
    public string mensagemTrancada = "A porta bateu com força e trancou por fora! Não há como sair agora.";

    public event Action OnMansaoEstadoMudou;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void DerrotarBoss()
    {
        if (bossDerrotado) return;
        bossDerrotado = true;
        Debug.Log("A ilusão foi quebrada! A mansão agora está na versão REAL.");
        OnMansaoEstadoMudou?.Invoke();
    }

    public bool EstaBloqueadoNaPortaPrincipal(string salaAtualID, float playerX)
    {
        if (bossDerrotado) return false;

        if (salaAtualID == idSalaSalao && Mathf.Abs(playerX - posicaoXPortaPrincipal) <= alcanceBloqueio)
        {
            Debug.Log(mensagemTrancada);
            return true;
        }

        return false;
    }
}