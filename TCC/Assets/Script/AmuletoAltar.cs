using UnityEngine;

public class AmuletoAltar : MonoBehaviour
{
    public static bool altarTocado = false;

    private bool jaColetado = false;

    private void Awake()
    {
        AtualizarVisibilidade();
    }

    private void OnEnable()
    {
        AtualizarVisibilidade();
    }

    private void Update()
    {
        // Garante que, se o altar estiver na mesma sala de algum amuleto, ele apareça na hora
        if (!jaColetado && altarTocado && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    private void AtualizarVisibilidade()
    {
        if (jaColetado)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(altarTocado);
    }

    // Ligue este método no 'Ao Interagir ()' do Altar
    public void MarcarAltarTocado()
    {
        altarTocado = true;
    }

    // Ligue este método no 'Ao Interagir ()' do próprio Amuleto (junto com a coleta)
    public void MarcarComoColetado()
    {
        jaColetado = true;
    }

    // Para resetar ao iniciar um novo jogo
    public static void Resetar()
    {
        altarTocado = false;
    }
}