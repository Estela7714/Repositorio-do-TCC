using UnityEngine;

public class ControleAcessoDelegacia : MonoBehaviour
{
    public static bool FalouComSenhoraDia2 = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Resetar()
    {
        FalouComSenhoraDia2 = false;
    }

    public void MarcarFalouComSenhoraDia2()
    {
        FalouComSenhoraDia2 = true;
    }
}