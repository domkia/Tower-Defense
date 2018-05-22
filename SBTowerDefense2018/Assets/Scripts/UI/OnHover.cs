using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;
    public GameObject Panel;
    public GameObject TowerPref;
    public Text text;
    //range, health, reload time, fire rate, ammo capacity

    public void Start()
    {
        Panel.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Panel.SetActive(true);
        BasicTower tower = TowerPref.GetComponent<BasicTower>();
        StringBuilder statsText = new StringBuilder();
        statsText.AppendLine("Health: " + tower.Health.ToString());
        statsText.AppendLine("Range: " + tower.Range);
        statsText.AppendLine("Reload Time: " + tower.ReloadTime);
        statsText.AppendLine("Fire Rate: " + tower.FireRate);
        statsText.AppendLine("Ammo: " + tower.ammoCapacity);
        text.text = statsText.ToString();
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Panel.SetActive(false);
        isOver = false;
    }
}