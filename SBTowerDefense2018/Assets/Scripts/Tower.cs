using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract tower class
/// </summary>
public abstract class Tower : MonoBehaviour, IDamagable<HexTile>
{
    [SerializeField]
    private int MaxHealth = 10;

    protected TowerInteractable towerInteractable;      // Reference to TowerInteractable component.
    public HexTile BuiltOn { get; protected set; }      // Tile this tower is built on
    protected List<HexTile> rangeTiles;                 // If enemy is within one of those tiles, it is added to the enemyList
    protected LinkedList<Enemy> enemyList;              // Enemies that are in range

    //IDamagable
    public event Action<HexTile> OnDeath;
    public int Health { get; set; }

    //TODO: Rename this method to Act or something else
    public abstract void Attack();
    public abstract float InteractionDuration { get; }

    //Protected stuff
    protected virtual void GetRangeTiles()
    {
        //Range tiles can be anything, for ex:
        rangeTiles = HexGrid.Instance.GetNeighbours(BuiltOn);           //By default range is 1
        //rangeTiles = HexGrid.Instance.GetTilesInRange(builtOn, 3);    //Get tiles of specific range
        //rangeTiles = HexGrid.Instance.GetTilesRing(builtOn, 2);       //Get ring of tiles
    }

    protected void Awake()
    {
        Health = MaxHealth;
        enemyList = new LinkedList<Enemy>();
        towerInteractable = GetComponentInChildren<TowerInteractable>();
    }

    public virtual void Setup(HexTile builtOn)
    {
        BuiltOn = builtOn;
        GetRangeTiles();                
        rangeTiles.Add(BuiltOn);        //Also don't forget to add tile tower is standing on

        SetupEnemyCallbacks();
    }

    protected virtual void Update()
    {
        Attack();
    }

    protected void SetupEnemyCallbacks()
    {
        if (rangeTiles == null || rangeTiles.Count == 0)
            Debug.LogError("This tower has no range tiles");
        for (int i = 0; i < rangeTiles.Count; i++)
        {
            rangeTiles[i].OnEnemyEnter += AddEnemyToQueue;
            rangeTiles[i].OnEnemyExit += RemoveEnemyFromQueue;
        }
    }

    /// <summary>
    /// Called when enemy finally goes out of range
    /// </summary>
    private void RemoveEnemyFromQueue(Enemy enemy, HexTile toTile)
    {
        if (!rangeTiles.Contains(toTile))
            enemyList.Remove(enemy);
    }

    private void RemoveEnemyOnDeath(Enemy enemy)
    {
        enemy.OnDeath -= RemoveEnemyOnDeath;
        enemyList.Remove(enemy);
    }

    private void AddEnemyToQueue(Enemy enemy)
    {
        if (enemyList.Contains(enemy))
            return;
        enemyList.AddLast(enemy);
        enemy.OnDeath += RemoveEnemyOnDeath;
    }

    //IDamagable
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("Tower destroyed!");
            if (OnDeath != null)
                OnDeath(BuiltOn);
        }
    }

    //Clean up
    private void OnDestroy()
    {
        for (int i = 0; i < rangeTiles.Count; i++)
        {
            rangeTiles[i].OnEnemyEnter -= AddEnemyToQueue;
            rangeTiles[i].OnEnemyExit -= RemoveEnemyFromQueue;
        }
    }

    /*
    // Debug enemy list
    private void OnGUI()
    {
        GUI.color = Color.blue;
        if (enemyList.Count == 0)
        {
            GUILayout.Label("enemyList is empty");
            return;
        }
        foreach(Enemy e in enemyList)
            GUILayout.Label(e.name);
    }*/
}