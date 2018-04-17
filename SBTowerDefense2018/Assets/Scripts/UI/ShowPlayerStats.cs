using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows player's stats after game over.
/// </summary>
public class ShowPlayerStats : MonoBehaviour {

    public Text KilledEnemyCountText;
    public Text WavesSurvivedText;

    private void Start()
    {
        KilledEnemyCountText.gameObject.SetActive(false);
        WavesSurvivedText.gameObject.SetActive(false);
        GameManager.OnGameWon += ShowStats;
        GameManager.OnGameOver += ShowStats;
    }

    private void ShowStats()
    {
        GameManager.OnGameWon -= ShowStats;
        GameManager.OnGameOver -= ShowStats;
        KilledEnemyCountText.text = string.Format("Enemies killed: {0}", PlayerStats.Instance.EnemiesKilled);
        WavesSurvivedText.text = string.Format("Waves survived: {0}", PlayerStats.Instance.WavesSurvived);
        KilledEnemyCountText.gameObject.SetActive(true);
        WavesSurvivedText.gameObject.SetActive(true);
    }

}
