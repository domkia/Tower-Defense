using UnityEngine;

public class GameManager : MonoBehaviour// : Singleton<GameManager>
{
    public delegate void GameStateHandler();
    public static GameStateHandler OnGameOver = delegate { };
    public static GameStateHandler OnGameWon = delegate { };

    private void Start()
    {
        //When the base is destroyed, GAME OVER
        Tower baseTower = TowerManager.Instance.GetTowerAt(HexGrid.Instance.CenterTile);
        baseTower.OnDeath += (a) => OnGameOver();

        //When all the enemies are killed, you WIN
        MonsterSpawner.OnAllEnemiesKilled += () => OnGameWon();
    }
}
