using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// This will hold the information (stats) for our player.
/// </summary>
[Serializable]
public class PlayerStats
{

    // I feel like this design is better: we don't even need to construct an instance of this in a game manager
    // or something for this work. If anyone disagrees, they may improve on it.
    private static PlayerStats instance = null;

    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = new PlayerStats();
            return instance;
        }
    }

    private PlayerStats()
    {
    }

    // Resources (wood, stone, iron, etc.) and their amounts.
    public Resource[] Resources { get; set; }

    // Amount of money the player has, which he earns from killing enemies.
    public int Money { get; private set; }

    // How many enemies the player has killed.
    public int EnemiesKilled { get; private set; }

    // How many towers the player has built.
    public int TowersBuilt { get; private set; }

    // How many seconds the player has survived in the current playing session.
    public int TimeSurvivedInSeconds { get; private set; }

    /// <summary>
    /// Changes the player's money by some amount.
    /// </summary>
    /// <param name="amount">Money amount</param>
    public void ChangeMoney(int amount)
    {
        Money += amount;
    }

    /// <summary>
    /// Increments the amount of enemies the player has killed by one.
    /// </summary>
    public void EnemyKilled()
    {
        EnemiesKilled++;
    }

    /// <summary>
    /// Increments the amount of towers the player has built by one.
    /// </summary>
    public void TowerBuilt()
    {
        TowersBuilt++;
    }

    /// <summary>
    /// Increments the amount of seconds the player has survived in the current playing session by one.
    /// </summary>
    public void IncrementSurvivalTimer()
    {
        TimeSurvivedInSeconds++;
    }

    private static readonly string filePath = Application.persistentDataPath + "/playerStats.dat";

    /// <summary>
    /// Saves the player's stats to a file.
    /// </summary>
    public void Save()
    {
        using (var file = File.Create(filePath))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(file, this);
        }
    }

    /// <summary>
    /// Loads the player's stats from a file.
    /// </summary>
    public void Load()
    {
        if(File.Exists(filePath))
        {
            using (var file = File.Open(filePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                var savedStats = (PlayerStats) formatter.Deserialize(file);
                Resources = savedStats.Resources;
                Money = savedStats.Money;
                EnemiesKilled = savedStats.EnemiesKilled;
                TowersBuilt = savedStats.TowersBuilt;
                TimeSurvivedInSeconds = savedStats.TimeSurvivedInSeconds;
            }
        }
    }
}