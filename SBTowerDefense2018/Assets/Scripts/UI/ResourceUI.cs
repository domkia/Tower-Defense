using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour {

    public Text ironText;
    public Text stoneText;
    public Text woodText;
	
	// Update is called once per frame
	void Update () {
        ironText.text = "Iron: " + PlayerStats.Instance.Resources[0].Amount;
        stoneText.text = "Stone: " + PlayerStats.Instance.Resources[1].Amount;
        woodText.text = "Wood: " + PlayerStats.Instance.Resources[2].Amount;
    }
}
