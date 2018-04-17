using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoneUI : MonoBehaviour {


    public Text stoneText;

    // Update is called once per frame
    void Update()
    {
        stoneText.text = "Stone: " + PlayerStats.Instance.Resources[1].Amount;
    }
}
