using UnityEngine;
using UnityEngine.InputSystem;

namespace Interact
{
    /// <summary>
    /// Sistema responsável por detectar objetos interagíveis e disparar interações.
    /// 
    /// Requisitos:
    /// - usar Collider2D como trigger para detectar objetos próximos
    /// - usar o New Input System (PlayerInput + InputAction "Interact")
    /// - registrar e remover callbacks no OnEnable/OnDisable
    /// 
    /// Fluxo esperado:
    /// - OnTriggerEnter2D → detectar e armazenar IInteractable atual + chamar OnEnterRange
    /// - OnTriggerExit2D → limpar referência + chamar OnExitRange
    /// - HandleInteract → chamado pelo input, executa OnInteract no alvo atual
    /// </summary>
    public class InteractSystem : MonoBehaviour
    {
        private IInteractable _current;

        void OnEnable() { }
        void OnDisable() { }

        void HandleInteract(InputAction.CallbackContext ctx) { }

        void OnTriggerEnter2D(Collider2D other) { }
        void OnTriggerExit2D(Collider2D other) { }
    }
    
}