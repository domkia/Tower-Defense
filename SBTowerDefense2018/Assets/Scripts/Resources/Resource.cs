using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Resource", menuName = "Resource", order = 0)]
public class Resource : ScriptableObject
{
    [SerializeField] public string resourceName;
    [SerializeField] public Sprite icon;

    public int Amount { get; private set; }

    //TODO: subscribe to this from ResourcesUI or something
    event Action OnNotEnoughResource;

    public void Add(int amount)
    {
        this.Amount += amount;
    }

    public bool Spend(int amount)
    {
        if (this.Amount > amount)
        {
            this.Amount -= amount;
            return true;
        }
        if (OnNotEnoughResource != null)
            OnNotEnoughResource();
        return false;
    }

    public void Reset()
    {
        this.Amount = 0;
    }
}