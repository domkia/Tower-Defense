using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTowerPanel : MonoBehaviour {

    public GameObject towerPanel;

    private void Start()
    {
        towerPanel.SetActive(false);
    }
    public void hidePanel()
    {
        towerPanel.SetActive(false);
    }
    public void showPanel()
    {
        towerPanel.SetActive(true);
    }

}
