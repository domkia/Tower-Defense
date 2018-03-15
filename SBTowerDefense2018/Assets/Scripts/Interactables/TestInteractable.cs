using System;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public float duration = 2f;
    private float currProgress;

    public event Action<IInteractable> OnCompleted;
    public event Action OnCancelled;

    public float UpdateProgress()
    {
        currProgress += (1f / duration) * Time.deltaTime;
        if (currProgress >= 1f)
        {
            currProgress = 0f;
            if (OnCompleted != null)
                OnCompleted(this);
            Debug.Log("Reloaded");
        }
        return currProgress;
    }

    public void Cancel()
    {
        currProgress = 0f;
        if (OnCancelled != null)
            OnCancelled();
    }
}
