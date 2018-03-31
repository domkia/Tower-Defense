using System;
using UnityEngine;

public class ResourceInteractable : MonoBehaviour, IInteractable
{
    public int maxTier = 3;           //How many times you can collect this resource until it's gone
    public int amountPerTier;         //How much you get for every tier
    public float timeToCollect;       //Time it takes to collect it once
    public Resource resource;         //Info about the resource

    private float currTime;
    private int currTier;

    public event Action<IInteractable> OnCompleted;     //Called when there is no more of this resource left
    public event Action OnCancelled;                    

    public event Action<IInteractable> OnCollected;     

    void Start()
    {
        currTime = 0f;
        currTier = maxTier;
        if (timeToCollect <= 0)
            Debug.LogError("timeToCollect illegal");
    }

    public void Cancel()
    {
        currTime = 0f;
        if (OnCancelled != null)
            OnCancelled();                      //Interaction was somehow cancelled
    }

    public float UpdateProgress()
    {
        currTime += Time.deltaTime;
        if (currTime > timeToCollect)
        {
            resource.Add(amountPerTier);        //Collect some
            currTime = 0f;
            if (OnCollected != null)
                OnCollected(this);
            currTier--;
            if (currTier <= 0)                  //Nothing left to collect
                Completed();
        }
        return currTime / timeToCollect;        //Returns the current progress
    }

    void Completed()
    {
        if(OnCompleted != null)
            OnCompleted(this);

        //TODO: Add particle effects, sounds etc.
        Destroy(this.gameObject);
    }

    // As long as the resource exists on the map, it is always interactive.
    public bool IsCurrentlyInteractive()
    {
        return true;
    }
}
