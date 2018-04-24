using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public GameObject gameStartPanel;

    void Start()
    {
        gameStartPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void startGame()
    {
        //bool isFull = false;
        //foreach(SpecialPower power in powersList)
        //{
        //    if (power == null)
        //        isFull = false;
        //    else isFull = true;
        //}
        //if (isFull)
        //{
            gameStartPanel.SetActive(false);
            Time.timeScale = 1;
        //}
    }
}
