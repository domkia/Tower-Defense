using System.Collections;
using UnityEngine;

public class LivesINFO : MonoBehaviour {
    public static int Lives;
    public int startLives = 12;
    void Start()
    {
       // Debug.Log(Lives);
        Lives = startLives;
       // Debug.Log(Lives);
    }
	
}
