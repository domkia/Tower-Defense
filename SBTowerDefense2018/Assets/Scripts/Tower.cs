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
    public List<HexTile> rangeTiles { get; protected set; } // If enemy is within one of those tiles, it is added to the enemyList
    protected Enemy currentTarget = null;

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

    protected virtual void Awake()
    {
        Health = MaxHealth;
        //enemyList = new LinkedList<Enemy>();
        towerInteractable = GetComponentInChildren<TowerInteractable>();
    }

    public virtual void Setup(HexTile builtOn)
    {
        BuiltOn = builtOn;
        GetRangeTiles();                
        rangeTiles.Add(BuiltOn);        //Also don't forget to add tile tower is standing on

        //SetupEnemyCallbacks();
    }

    /// <summary>
    /// Finds nearest enemy in range and updates target. If there is no viable enemy, target is set to null
    /// (indicating no target)
    /// </summary>
    protected virtual void UpdateTarget()
    {
        int index = -1;
        float nearDist = float.MaxValue;
        for (int i = 0; i < rangeTiles.Count; i++)
            if (rangeTiles[i].enemies.Count > 0)
            {
                float dist = (BuiltOn.worldPos - rangeTiles[i].enemies[0].transform.position).sqrMagnitude;
                if (dist < nearDist)
                {
                    nearDist = dist;
                    index = i;
                }
            }
        if (index < 0)
            currentTarget = null;
        else
            currentTarget = rangeTiles[index].enemies[0];
    }

    protected virtual void Update()
    {
        if (currentTarget == null)
            UpdateTarget();
        if(currentTarget != null)
            Attack();
    }

    //IDamagable
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        healthbar.fillAmount = (float)Health / (float)MaxHealth;
        if (Health <= 0)
        {
            Debug.Log("Tower destroyed!");
            if (OnDeath != null)
                OnDeath(BuiltOn);
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

    //Temporary
    //TODO: move this
    public UnityEngine.UI.Image healthbar;
}