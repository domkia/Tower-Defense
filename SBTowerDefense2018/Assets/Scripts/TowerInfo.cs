using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour {

    public static TowerInfo selected;

    public GameObject infoWindow;

    private GameObject window;

    private void OnMouseDown()
    {
        if (!infoWindow.gameObject.activeSelf)
        { 
            Debug.Log("Paspaustas");
            if (TowerInfo.selected != null)
            {
                TowerInfo.selected.Hide();
            }

            TowerInfo.selected = this;
            infoWindow.gameObject.SetActive(true);
        }
        else if (infoWindow.gameObject.activeSelf)
        {
            Debug.Log("Paspaustas");
            if (TowerInfo.selected != null)
            {
                TowerInfo.selected.Hide();
            }

            TowerInfo.selected = this;
            infoWindow.gameObject.SetActive(false);
        }
    }

    public void Hide()
    {
        Debug.Log("Hide");
        infoWindow.gameObject.SetActive(false);
    }

    public void Setup(params InfoField[] fields)
    {
        GameObject canvas = GameObject.Find("Canvas");
        window = Instantiate(infoWindow) as GameObject;
        window.transform.parent = canvas.transform;
        foreach (var field in fields)
        {
            GameObject rowObj = new GameObject();
            rowObj.transform.parent = window.transform;

            rowObj.AddComponent<LayoutElement>();

            Text row = rowObj.AddComponent<Text>();
            row.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            row.text = string.Format("{0}\t{1}", field.name, field.value);
        }
        Hide();
    }
}

public struct InfoField
{
    public string name;
    public string value;

    public InfoField(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
}

