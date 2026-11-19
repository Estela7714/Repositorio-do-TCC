using System;
using System.Collections;
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

    [Header("Configuração de Digitação")]
    [Tooltip("Tempo em segundos entre cada letra (ex: 0.03 = rápido, 0.06 = mais devagar)")]
    [SerializeField] private float velocidadeDigitacao = 0.04f;

    public static bool Aberto => Instance != null && Instance.painelDialogo != null && Instance.painelDialogo.activeSelf;

    private Queue<LinhaDialogo> filaFalas = new Queue<LinhaDialogo>();
    private Action aoTerminarCallback;
    private bool iniciouNesteFrame;
    private bool digitando = false;
    private Coroutine corrotinaDigitacao;

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

        // Bloqueia qualquer clique ou tecla enquanto as letras ainda estão aparecendo
        if (digitando) return;

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
        if (digitando) return;

        if (filaFalas.Count == 0)
        {
            FecharDialogo();
            return;
        }

        LinhaDialogo atual = filaFalas.Dequeue();

        if (textoNome != null)
            textoNome.text = atual.nomeFalante;

        if (imagemRetrato != null)
        {
            imagemRetrato.gameObject.SetActive(atual.retrato != null);
            imagemRetrato.sprite = atual.retrato;
        }

        if (corrotinaDigitacao != null)
            StopCoroutine(corrotinaDigitacao);

        corrotinaDigitacao = StartCoroutine(DigitarTexto(atual.fala));
        if (textoNome != null)
        {
            bool temNome = !string.IsNullOrWhiteSpace(atual.nomeFalante);
            textoNome.gameObject.SetActive(temNome);
            textoNome.text = atual.nomeFalante;
        }
    }

    private IEnumerator DigitarTexto(string fraseCompleta)
    {
        digitando = true;

        if (textoFala != null)
        {
            textoFala.text = fraseCompleta;
            textoFala.maxVisibleCharacters = 0;

            int totalCaracteres = fraseCompleta.Length;
            for (int i = 1; i <= totalCaracteres; i++)
            {
                textoFala.maxVisibleCharacters = i;
                yield return new WaitForSeconds(velocidadeDigitacao);
            }
        }

        digitando = false;
    }

    private void FecharDialogo()
    {
        if (corrotinaDigitacao != null)
        {
            StopCoroutine(corrotinaDigitacao);
            corrotinaDigitacao = null;
        }

        digitando = false;
        painelDialogo.SetActive(false);
        aoTerminarCallback?.Invoke();
        aoTerminarCallback = null;
    }
}