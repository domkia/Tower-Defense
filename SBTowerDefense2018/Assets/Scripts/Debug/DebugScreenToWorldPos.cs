using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DebugScreenToWorldPos : MonoBehaviour
{
    private Text debugText;
    private Ray ray;
    private RaycastHit hit;

    private void Start()
    {
        debugText = GetComponent<Text>();
    }

    void Update ()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            debugText.text = string.Format("Hit '{0}' at: {1}", hit.collider.name, hit.point);
            debugText.color = Color.green;
        }
        else
        {
            debugText.text = "Hit nothing..";
            debugText.color = Color.red;
        }
	}
}
