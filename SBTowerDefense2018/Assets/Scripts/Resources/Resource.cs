using System.Runtime.Serialization;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Resource", menuName = "Resource", order = 0)]
[Serializable]
public class Resource : ScriptableObject
{
    // Name of the resource ("Wood", "Stone", and so on)
    [SerializeField] public string ResourceName;
    // Its icon in the UI.
    // TO DO: Figure out a way to serialize a sprite. (Do we even need to serialize it anyway?)
    // [SerializeField] public Sprite Icon;

    // The amount of this particular resource the player holds at the moment.
    public int Amount { get; private set; }

    // TODO: subscribe to this from ResourcesUI or something
    event Action OnNotEnoughResource;

    // Used to update UI elements.
    public static event Action OnChangedAmount = delegate { };

    /// <summary>
    /// Adds some amount of resource to the player.
    /// </summary>
    /// <param name="amount">Amount of resource</param>
    public void Add(int amount)
    {
        this.Amount += amount;
        OnChangedAmount();
    }

    /// <summary>
    /// Takes away some amount of resource from the player.
    /// </summary>
    /// <param name="amount">Amount of resource</param>
    /// <returns>true, if the player had enough resource to spend, otherwise false.</returns>
    public bool Spend(int amount)
    {
        if (this.Amount >= amount)
        {
            this.Amount -= amount;
            OnChangedAmount();
            return true;
        }
        if (OnNotEnoughResource != null)
            OnNotEnoughResource();
        return false;
    }

    /// <summary>
    /// Sets this resource's amount to 0.
    /// </summary>
    public void Reset()
    {
        this.Amount = 0;
    }
}