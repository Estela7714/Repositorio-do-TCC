using UnityEngine;
using UnityEngine.InputSystem; // Adicionado o namespace novo

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float speed = 3f;

    private float minWalkX;
    private float maxWalkX;
    private float horizontalInput;

    private void Update()
    {
        horizontalInput = 0f;

        // Verifica se o teclado está conectado
        if (Keyboard.current != null)
        {
            // Checa botões A ou Seta Esquerda
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontalInput = -1f;
            }
            // Checa botões D ou Seta Direita
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                horizontalInput = 1f;
            }
        }

        if (horizontalInput != 0f)
        {
            float currentX = transform.position.x;
            float newX = currentX + (horizontalInput * speed * Time.deltaTime);

            newX = Mathf.Clamp(newX, minWalkX, maxWalkX);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    public void SetLimits(float minX, float maxX)
    {
        minWalkX = minX;
        maxWalkX = maxX;
    }

    public void StopMovement()
    {
        horizontalInput = 0f;
    }
}