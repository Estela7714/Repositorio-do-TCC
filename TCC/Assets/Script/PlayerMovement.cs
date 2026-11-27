using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float speed = 3f;

    [Header("Direção do Sprite")]
    [Tooltip("Marque se o desenho original do personagem (Scale X positivo) olha para a ESQUERDA.")]
    [SerializeField] private bool spriteOlhaParaEsquerda = false;

    private float minWalkX;
    private float maxWalkX;
    private float horizontalInput;

    private void Update()
    {
        // Não anda enquanto uma mensagem/descrição ou diálogo está na tela
        if (MensagemUI.Aberta || DialogoManager.Aberto)
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
            AtualizarDirecao(horizontalInput);

            float currentX = transform.position.x;
            float newX = currentX + (horizontalInput * speed * Time.deltaTime);

            newX = Mathf.Clamp(newX, minWalkX, maxWalkX);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    private void AtualizarDirecao(float direcao)
    {
        // Preserva o valor absoluto da escala definida pelo RoomManager em cada sala
        float escalaAtual = Mathf.Abs(transform.localScale.x);
        if (escalaAtual == 0f) escalaAtual = 1f;

        float sinal = direcao > 0f ? 1f : -1f;
        if (spriteOlhaParaEsquerda) sinal = -sinal;

        transform.localScale = new Vector3(escalaAtual * sinal, transform.localScale.y, transform.localScale.z);
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