using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Substitua "Gameplay" pelo nome exato da sua cena no Unity
    [SerializeField] private string nomeDaCenaGameplay = "Gameplay";

    // Associe este método ao evento OnClick() do botão de Play no Canvas
    public void IniciarJogo()
    {
        SceneManager.LoadScene(nomeDaCenaGameplay);
    }
}