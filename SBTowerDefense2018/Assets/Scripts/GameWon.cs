using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        text.gameObject.SetActive(false);
        MonsterSpawner.onAllEnemiesKilled += Won;
    }

    void Won()
    {
        text.gameObject.SetActive(true);
    }
}
