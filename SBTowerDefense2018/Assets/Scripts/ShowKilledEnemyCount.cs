using UnityEngine;
using UnityEngine.UI;

public class ShowKilledEnemyCount : MonoBehaviour {

    public Text KilledEnemyCountText;

    private void Start()
    {
        KilledEnemyCountText.gameObject.SetActive(false);
        GameManager.OnGameWon += ShowText;
        GameManager.OnGameOver += ShowText;
    }

    private void ShowText()
    {
        GameManager.OnGameWon -= ShowText;
        GameManager.OnGameOver -= ShowText;
        KilledEnemyCountText.text = string.Format("Enemies killed: {0}", PlayerStats.Instance.EnemiesKilled);
        KilledEnemyCountText.gameObject.SetActive(true);
    }

}
