using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLost : MonoBehaviour {
    private bool gameEnded = false;
         
	void Update () {
        if (gameEnded)
            return;
		if (LivesINFO.Lives <= 0)
        {
            EndGame();
        }
	}

    void EndGame()
    {
        gameEnded = true;
        Debug.Log("GAME OVER!");
    }
}
