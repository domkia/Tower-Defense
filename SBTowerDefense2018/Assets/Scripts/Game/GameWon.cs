using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        text.gameObject.SetActive(false);
        GameManager.OnGameWon += Won;
    }

    void Won()
    {
        text.gameObject.SetActive(true);
        GameManager.OnGameWon -= Won;
        Time.timeScale = 0;
    }
}
