using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IronUI : MonoBehaviour {

    public Text ironText;
	
	// Update is called once per frame
	void Update () {
        ironText.text = "Iron: " + PlayerStats.Instance.Resources[0].Amount;
	}
}
