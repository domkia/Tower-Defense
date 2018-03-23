using System;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "new Resource", menuName = "Resource", order = 0)]
[Serializable]
public class Resource : ScriptableObject, ISerializable
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

    // TODO: when the amount of a resource changes, for example, when the player collects a resource,
    // update the UI.
    event Action OnChangedAmount;

    /// <summary>
    /// Adds some amount of resource to the player.
    /// </summary>
    /// <param name="amount">Amount of resource</param>
    public void Add(int amount)
    {
        this.Amount += amount;
        if (OnChangedAmount != null)
            OnChangedAmount();
    }

    /// <summary>
    /// Takes away some amount of resource from the player.
    /// </summary>
    /// <param name="amount">Amount of resource</param>
    /// <returns>true, if the player had enough resource to spend, otherwise false.</returns>
    public bool Spend(int amount)
    {
        if (this.Amount > amount)
        {
            this.Amount -= amount;
            if (OnChangedAmount != null)
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

    // This method is called on serialization.
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("resourceName", ResourceName, typeof(string));
        info.AddValue("amount", Amount, typeof(int));
    }

    // This special constructor is used for deserialization.
    public Resource(SerializationInfo info, StreamingContext context)
    {
        ResourceName = (string) info.GetValue("resourceName", typeof(string));
        Amount = (int) info.GetValue("amount", typeof(int));
    }
}