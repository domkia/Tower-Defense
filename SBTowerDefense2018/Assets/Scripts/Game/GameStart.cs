using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : Singleton<GameStart>
{
    public bool isFull;
    public static List<SpecialAttack> specialList = new List<SpecialAttack>(3); //3 chosen abilities
    public GameObject gameStartPanel;
    public GameObject[] Buttons = new GameObject[3];

    void Start()
    {
        specialList = Enumerable.Repeat<SpecialAttack>(null, 3).ToList();
        gameStartPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void startGame()
    {
        isFull = false;
        foreach(SpecialAttack power in specialList)
        {
           if (power == null)
                isFull = false;
           else isFull = true;
        }
        if (isFull)
        {
            gameStartPanel.SetActive(false);
            Time.timeScale = 1;
            for(int i = 0; i < 3; i++)
            {
                Buttons[i].GetComponentInChildren<Text>().text = specialList[i].name;
            }
           
        }
    }
}
