using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float speed = 3f;

    private float minWalkX;
    private float maxWalkX;
    private float horizontalInput;

    private void Update()
    {
        // NOVO: não anda enquanto uma mensagem/descrição está na tela
        if (MensagemUI.Aberta)
        {
            horizontalInput = 0f;
            return;
        }

        horizontalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontalInput = -1f;
            }
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