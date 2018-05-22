using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour {

    public Text ironText;
    public Text stoneText;
    public Text woodText;

    private void Start()
    {
        Resource.OnChangedAmount += UpdateText;
        ResetText();
    }

    private void UpdateText()
    {
        ironText.text = "Iron: " + PlayerStats.Instance.Resources[0].Amount;
        stoneText.text = "Stone: " + PlayerStats.Instance.Resources[1].Amount;
        woodText.text = "Wood: " + PlayerStats.Instance.Resources[2].Amount;
    }

    private void ResetText()
    {
        ironText.text = "Iron: 0";
        stoneText.text = "Stone: 0";
        woodText.text = "Wood: 0";
    }
}
