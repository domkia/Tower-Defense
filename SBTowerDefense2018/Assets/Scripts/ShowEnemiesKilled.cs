using UnityEngine;
using UnityEngine.UI;

public class ShowEnemiesKilled : MonoBehaviour
{
    public Text EnemiesKilledText;

    private void Start()
    {
        EnemiesKilledText.gameObject.SetActive(false);
        GameManager.OnGameWon += ShowText;
        GameManager.OnGameOver += ShowText;
    }

    private void ShowText()
    {
        GameManager.OnGameWon -= ShowText;
        GameManager.OnGameOver -= ShowText;
        EnemiesKilledText.text = string.Format("Enemies killed: {0}", PlayerStats.Instance.EnemiesKilled);
        EnemiesKilledText.gameObject.SetActive(true);
    }
}