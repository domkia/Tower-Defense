using System;
using UnityEngine;

/// <summary>
/// Tower component, responsible for the interactive nature of the tower.
/// </summary>
public class TowerInteractable : MonoBehaviour, IInteractable
{
    // Reference to tower which this component is part of.
    private Tower parent;
    // Current reloading progress [0-1]
    private float currentProgress;
    // Indicates whether the parent tower is currently interactive or not.
    private bool isCurrentlyInteractive;

    /// <summary>
    /// Sets the parent tower of this component. (Set to this)
    /// </summary>
    /// <param name="tower">Parent tower (this)</param>
    public void SetParent(Tower tower)
    {
        parent = tower;
    }

    /// <summary>
    /// Makes the tower interactive. Call this method when the tower has no bullets left.
    /// </summary>
    public void SetToInteractive()
    {
        isCurrentlyInteractive = true;
    }

    #region IInteractable implementation

    public event Action<IInteractable> OnCompleted;
    public event Action OnCancelled;

    public float UpdateProgress()
    {
        currentProgress += (1f / parent.InteractionDuration) * Time.deltaTime;
        if(currentProgress >= 1f)
        {
            currentProgress = 0f;
            //parent.Reload();                  // What if tower does something else instead of reloading?
            isCurrentlyInteractive = false;
            if (OnCompleted != null)
                OnCompleted(this);              // call Reload() or any other method from DERIVED classes
        }
        return currentProgress;
    }

    public void Cancel()
    {
        currentProgress = 0f;
        if (OnCancelled != null)
            OnCancelled();
    }

    public bool IsCurrentlyInteractive()
    {
        return isCurrentlyInteractive;
    }

    #endregion
}