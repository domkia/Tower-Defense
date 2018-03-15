using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UpdateResourceUI : MonoBehaviour {

    // Reference to the text on screen that displays the resources and their amounts.
    public Text ResourceUIText;

	private void Start() {
		foreach(var resource in PlayerStats.Instance.Resources)
            resource.Reset();

        var temp = new StringBuilder();
        foreach (var resource in PlayerStats.Instance.Resources)
            temp.AppendFormat("{0}: {1}\t", resource.ResourceName, resource.Amount);

        ResourceUIText.text = temp.ToString();
	}
	
	// TODO: replace this with a callback system, so when the player earns or spends some resource,
    // update the UI then, rather than every single frame.
	private void Update() {
        var temp = new StringBuilder();
        foreach (var resource in PlayerStats.Instance.Resources)
            temp.AppendFormat("{0}: {1}\t", resource.ResourceName, resource.Amount);

        ResourceUIText.text = temp.ToString();
    }
}
