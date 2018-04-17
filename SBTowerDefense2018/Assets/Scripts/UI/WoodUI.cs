using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WoodUI : MonoBehaviour {


    public Text woodText;

    // Update is called once per frame
    void Update()
    {
        woodText.text = "Wood: " + PlayerStats.Instance.Resources[2].Amount;
    }
}
