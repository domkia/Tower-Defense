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

    public PlayerStats()
    {
        //Load all Resource scriptable objects from unity's built in Resources folder
        this.Resources = UnityEngine.Resources.LoadAll<Resource>("ResourcesInfo/");
    }

    // Resources (wood, stone, iron, etc.) and their amounts.
    public Resource[] Resources { get; private set; }

    // Amount of money the player has, which he earns from killing enemies.
    public int Money { get; private set; }

    // How many enemies the player has killed.
    public int EnemiesKilled { get; private set; }

    // How many towers the player has built.
    public int TowersBuilt { get; private set; }

    // How many waves has the player survived.
    public int WavesSurvived { get; private set; }

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
    /// Increments the amount of wave the player has survived.
    /// </summary>
    public void WaveSurvived()
    {
        WavesSurvived++;
    }
    public void resetResources()
    {
        foreach(Resource res in Resources)
        {
            res.Reset();
        }
    }

    private static readonly string filePath = "/playerStats.dat";

    /// <summary>
    /// Saves the player's stats to a file.
    /// </summary>
    public void Save()
    {
        using (var file = File.Create(Application.persistentDataPath + filePath))
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
        if(File.Exists(Application.persistentDataPath + filePath))
        {
            using (var file = File.Open(Application.persistentDataPath + filePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                var savedStats = (PlayerStats) formatter.Deserialize(file);
                Resources = savedStats.Resources;
                Money = savedStats.Money;
                EnemiesKilled = savedStats.EnemiesKilled;
                TowersBuilt = savedStats.TowersBuilt;
                WavesSurvived = savedStats.WavesSurvived;
            }
        }
    }
}