using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public struct LinhaDialogo
{
    public string nomeFalante;
    public Sprite retrato; // Opcional (deixe vazio se não usar)
    [TextArea(2, 5)] public string fala;
}

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance { get; private set; }

    [Header("Componentes da UI")]
    [SerializeField] private GameObject painelDialogo;
    [SerializeField] private TextMeshProUGUI textoNome;
    [SerializeField] private TextMeshProUGUI textoFala;
    [SerializeField] private Image imagemRetrato; // Opcional

    public static bool Aberto => Instance != null && Instance.painelDialogo != null && Instance.painelDialogo.activeSelf;

    private Queue<LinhaDialogo> filaFalas = new Queue<LinhaDialogo>();
    private Action aoTerminarCallback;
    private bool iniciouNesteFrame;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (painelDialogo != null)
            painelDialogo.SetActive(false);
    }

    private void Update()
    {
        if (!Aberto || Time.timeScale == 0f) return;

        // Evita que o mesmo clique que abriu o diálogo já pule a primeira fala
        if (iniciouNesteFrame)
        {
            iniciouNesteFrame = false;
            return;
        }

        // Avança o diálogo ao clicar com o botão esquerdo do mouse ou apertar Espaço
        bool clicou = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool apertouEspaco = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        if (clicou || apertouEspaco)
        {
            ProximaFala();
        }
    }

    public void IniciarDialogo(LinhaDialogo[] linhas, Action aoTerminar = null)
    {
        if (linhas == null || linhas.Length == 0) return;

        filaFalas.Clear();
        foreach (LinhaDialogo linha in linhas)
        {
            filaFalas.Enqueue(linha);
        }

        aoTerminarCallback = aoTerminar;
        painelDialogo.SetActive(true);
        iniciouNesteFrame = true;

        ProximaFala();
    }

    public void ProximaFala()
    {
        if (filaFalas.Count == 0)
        {
            FecharDialogo();
            return;
        }

        LinhaDialogo atual = filaFalas.Dequeue();

        if (textoNome != null)
            textoNome.text = atual.nomeFalante;

        if (textoFala != null)
            textoFala.text = atual.fala;

        if (imagemRetrato != null)
        {
            imagemRetrato.gameObject.SetActive(atual.retrato != null);
            imagemRetrato.sprite = atual.retrato;
        }
    }

    private void FecharDialogo()
    {
        painelDialogo.SetActive(false);
        aoTerminarCallback?.Invoke();
        aoTerminarCallback = null;
    }
}