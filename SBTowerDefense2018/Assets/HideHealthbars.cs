using UnityEngine;

public class HideHealthbars : MonoBehaviour
{

	void Start ()
    {
        GameManager.OnGameOver += () => gameObject.SetActive(false);
        GameManager.OnGameWon += () => gameObject.SetActive(false);
    }

}
