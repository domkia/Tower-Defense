using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{

    public Text text;

    private void Start()
    {
        text.gameObject.SetActive(false);
        MonsterSpawner.onFinishedSpawning += Won;
    }

    void Won()
    {
        text.gameObject.SetActive(true);
    }
}
