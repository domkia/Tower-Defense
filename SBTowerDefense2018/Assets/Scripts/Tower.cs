using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract tower class
/// </summary>
public abstract class Tower : MonoBehaviour
{
    protected TowerInteractable towerInteractable;      // Reference to TowerInteractable component.
    public HexTile BuiltOn { get; protected set; }      // Tile this tower is built on
    protected List<HexTile> rangeTiles;
    protected LinkedList<Enemy> enemyList;              // Enemies that are in range

    public abstract void Attack();
    public abstract float InteractionDuration { get; }

    //Protected stuff
    protected virtual void GetRangeTiles()
    {
        //Range tiles can be anything, for ex:
        rangeTiles = HexGrid.Instance.GetNeighbours(BuiltOn);           //By default range is 1
        //rangeTiles = HexGrid.Instance.GetTilesInRange(builtOn, 3);    //Get tiles of specific range
        //rangeTiles = HexGrid.Instance.GetTilesRing(builtOn, 2);       //Get ring of tiles
        SetupEnemyCallbacks();
    }

    protected virtual void Start()
    {
        enemyList = new LinkedList<Enemy>();
        towerInteractable = GetComponentInChildren<TowerInteractable>();
        GetRangeTiles();
    }

    protected virtual void Update()
    {
        Attack();
    }

    protected void SetupEnemyCallbacks()
    {
        if (rangeTiles == null || rangeTiles.Count == 0)
            return;
        for (int i = 0; i < rangeTiles.Count; i++)
        {
            rangeTiles[i].OnEnemyEnter += AddEnemyToQueue;
            rangeTiles[i].OnEnemyExit += RemoveEnemyFromQueue;
        }
    }

    //Private
    private void RemoveEnemyFromQueue(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }

    private void AddEnemyToQueue(Enemy enemy)
    {
        enemyList.AddLast(enemy);
        enemy.OnDeath += RemoveEnemyFromQueue;
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
}