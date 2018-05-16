using System;
using UnityEngine;

public class ResourceInteractable : MonoBehaviour, IInteractable, ISelectable
{
    public Color selectionColor;
    public int maxTier = 3;           //How many times you can collect this resource until it's gone
    public int amountPerTier;         //How much you get for every tier
    public float timeToCollect;       //Time it takes to collect it once
    public Resource resource;         //Info about the resource
    public GameObject gatherParticle;
    public GameObject finishedParticle;

    private float currTime;
    private int currTier;

    public Color SelectionColor { get { return selectionColor; } }

    // Sound stuff
    private PlayResourceCollectingSFX soundPlayer;
    private float timeBetweenSFX = 0.6f;
    private float timeUntilSFX = 0.0f;
    // -----------
    //World pos

    public event Action<IInteractable> OnCompleted;     //Called when there is no more of this resource left
    public event Action<IInteractable> OnCancelled;                    

    public event Action<IInteractable> OnCollected;     

    void Start()
    {
        currTime = 0f;
        currTier = maxTier;
        if (timeToCollect <= 0)
            Debug.LogError("timeToCollect illegal");
        soundPlayer = GetComponent<PlayResourceCollectingSFX>();
    }

    public void Cancel()
    {
        currTime = 0f;
        if (OnCancelled != null)
            OnCancelled(this);                      //Interaction was somehow cancelled
    }

    public float UpdateProgress()
    {
        currTime += Time.deltaTime;

        timeUntilSFX -= Time.deltaTime;
        if(timeUntilSFX <= 0.0f)
        {
            
            soundPlayer.PlaySound(SoundType.ResourceCollecting);
            Instantiate(gatherParticle, GetComponentInParent<Transform>().position, GetComponentInParent<Transform>().rotation);
            timeUntilSFX = timeBetweenSFX;
        }

        if (currTime > timeToCollect)
        {
            resource.Add(amountPerTier);        //Collect some
            soundPlayer.PlaySound(SoundType.ResourceCollected);
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
        Debug.Log("finished");
        float delay = soundPlayer.PlaySound(SoundType.ResourceDepleted);        
        // Hide the game object.
        gameObject.GetComponentInChildren<Renderer>().enabled = false;
        //Particles when resource is completed
        Instantiate(finishedParticle, GetComponentInParent<Transform>());
        // Delay destroying the game object until the resource depletion sound effect has finished playing.
        // Otherwise the sound effect would cut out.
        Destroy(gameObject, delay);
    }

    // As long as the resource exists on the map, it is always interactive.
    public bool IsCurrentlyInteractive()
    {
        return true;
    }
}
