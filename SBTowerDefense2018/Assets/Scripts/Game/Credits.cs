using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Credits : MonoBehaviour {

    public GameObject text;

    private void Start()
    {
        text.gameObject.SetActive(false);
    }

    public void showtext()
    {
        text.gameObject.SetActive(true);
    }
    public void hidetext()
    {
        text.gameObject.SetActive(false);
    }
}
