using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Lê os cliques do mouse e decide o que fazer.
// Clique esquerdo: interagir / usar item selecionado. Clique direito: desselecionar item.
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask camadaInteragivel = ~0; // por padrão, tudo

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return; // jogo pausado

        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        // Botão direito: guarda o item (volta para a "mão vazia")
        if (mouse.rightButton.wasPressedThisFrame)
        {
            Inventory.Instance.Selecionar(null);
            return;
        }

        if (!mouse.leftButton.wasPressedThisFrame) return;

        // Se há mensagem aberta, o clique só fecha ela
        if (MensagemUI.Aberta)
        {
            MensagemUI.Fechar();
            return;
        }

        // Ignora cliques em cima de botões/UI (inventário, pause etc.)
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Vector2 mundo = mainCamera.ScreenToWorldPoint(mouse.position.ReadValue());
        Collider2D hit = Physics2D.OverlapPoint(mundo, camadaInteragivel);
        if (hit == null) return;

        ObjetoInteragivel objeto = hit.GetComponentInParent<ObjetoInteragivel>();
        if (objeto == null) return;

        ItemData selecionado = Inventory.Instance.ItemSelecionado;
        if (selecionado != null) objeto.UsarItem(selecionado);
        else objeto.Interagir();
    }
}