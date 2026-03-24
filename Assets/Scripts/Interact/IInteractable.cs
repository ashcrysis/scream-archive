namespace Interact
{
    /// <summary>
    /// ‘Interface’ base para qualquer objeto interagível.
    /// 
    /// Implementação esperada:
    /// - OnEnterRange → ‘feedback’ visual (outline, UI, etc)
    /// - OnExitRange → remover ‘feedback’
    /// - OnInteract → executar ação principal (dialogo, loot, etc)
    /// </summary>
    public interface IInteractable
    {
        void OnInteract();
        void OnEnterRange();
        void OnExitRange();
    }
}