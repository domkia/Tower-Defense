using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public delegate void GameStateHandler();
    public static GameStateHandler OnGameOver = delegate { };
    public static GameStateHandler OnGameWon = delegate { };

    private void Start()
    {
        //Resets resources when game is started
        PlayerStats.Instance.ResetResources(); 
        //When the base is destroyed, GAME OVER
        Tower baseTower = TowerManager.Instance.GetTowerAt(HexGrid.Instance.CenterTile);
        baseTower.OnDeath += (a) => OnGameOver();

        //When all the enemies are killed, you WIN
        MonsterSpawner.OnAllEnemiesKilled += () => OnGameWon();
    }
}
