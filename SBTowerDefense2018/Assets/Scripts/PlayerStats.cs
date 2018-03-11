/// <summary>
/// This will hold the information (stats) for our player.
/// </summary>
[System.Serializable]
public class PlayerStats
{

    public static PlayerStats Instance { get; private set; }

    // Amount of money that the player currently has, which can later be used to buy and upgrade towers.
    // *** THIS WILL PROBABLY CHANGE LATER. ***
    public int Money { get; private set; }

    // How many enemies the player has killed.
    public int EnemiesKilled { get; private set; }

    // How many towers the player has built.
    public int TowersBuilt { get; private set; }

    // How many seconds the player has survived in the current playing session.
    public int TimeSurvivedInSeconds { get; private set; }

    public PlayerStats()
    {
        Instance = this;
    }

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
}