using System;

public interface IInteractable
{
    /// <summary>
    /// Indicates whether this interactable is currently interactive or not. Not all
    /// interactables are interactive all the time. For example, a tower is only interactive
    /// when it has no bullets left, otherwise it is non-interactive.
    /// </summary>
    /// <returns></returns>
    bool IsCurrentlyInteractive();

    float UpdateProgress();
    void Cancel();
    event Action<IInteractable> OnCompleted;
    event Action<IInteractable> OnCancelled;
}
