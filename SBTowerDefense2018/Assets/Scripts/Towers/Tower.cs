using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for towers that will be built by the player.
/// </summary>
[RequireComponent(typeof(Healthbar))]
[RequireComponent(typeof(TowerInteractable))]
public abstract class Tower : MonoBehaviour, IDamagable<HexTile>
{
    // Current health of the tower.
    public int CurrentHealth { get; set; }

    // Maximum amount of health the tower will have.
    // We need this in order to draw the healthbar.
    public int Health;

    // Resource costs to build the tower.
    public int WoodCost = 0;
    public int IronCost = 0;
    public int StoneCost = 0;

    // Tile the tower is built on.
    public HexTile BuiltOn { get; protected set; }

    // Range of this tower (in tiles)
    public int Range;

    // All tiles in range of this tower. Any enemy that enters any of the tiles in this list,
    // becomes a viable target.
    public List<HexTile> TilesInRange { get; protected set; }

    // This is called when the tower is destroyed.
    public event Action<HexTile> OnDeath = delegate { };

    // Reference to TowerInteractable component.
    protected TowerInteractable towerInteractable;

    // Reference to Healthbar component.
    protected Healthbar healthbar;

    public abstract float InteractionDuration { get; }

    /// <summary>
    /// Sets up the list, which holds the tiles in range for this tower.
    /// In the base class (Tower), this only adds the tile the tower is built on.
    /// </summary>
    protected virtual void SetupTilesInRange()
    {
        TilesInRange = new List<HexTile> { BuiltOn };
    }

    protected virtual void Awake()
    {
        CurrentHealth = Health;
        towerInteractable = GetComponentInChildren<TowerInteractable>();
        healthbar = GetComponent<Healthbar>();
    }

    public virtual void Setup(HexTile builtOn)
    {
        BuiltOn = builtOn;
        SetupTilesInRange();
    }

    /// <summary>
    /// Deals damage to this tower.
    /// </summary>
    /// <param name="damage">Amount of damage this tower will take.</param>
    public virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        healthbar.UpdateHealthbar(CurrentHealth, Health);
        if (CurrentHealth <= 0)
            OnDeath(BuiltOn);
    }
}