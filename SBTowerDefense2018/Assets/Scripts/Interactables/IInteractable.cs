using System;

public interface IInteractable
{
    float UpdateProgress();
    void Cancel();
    event Action<IInteractable> OnCompleted;
    event Action OnCancelled;
}
