using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesManager : MonoBehaviour {

    private List<SpecialAttack> specialAttack = new List<SpecialAttack>();
    private List<Image> cooldowns = new List<Image>();

    public GameObject abilityChoosePanel;
    public GameObject[] selectionButtons = new GameObject[3];
    private int current;
    // Use this for initialization
    void Start () {
        abilityChoosePanel.SetActive(false);
        ReloadSpecial reload = new ReloadSpecial();
        specialAttack.Add(reload);
        IncreaseDamage damage = new IncreaseDamage();
        specialAttack.Add(damage);
        FreezeEnemy freeze = new FreezeEnemy();
        specialAttack.Add(freeze);
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameStart.Instance.isFull)
            return;
        for(int i = 0; i < specialAttack.Count; i++)
        {
           
            specialAttack[i].UpdateCooldown(); 
            cooldowns[i].fillAmount = specialAttack[i].timer / specialAttack[i].cooldown;
        }
		
        if (Input.GetKeyDown(KeyCode.Space))
        {
            specialAttack[0].Do();
        }
	}
    public void doSpecialAttack(int index)
    {
        if (index < 0 || index > specialAttack.Count)
        {
            Debug.Log("Index out of bounds");
            return;
        }
        else
        GameStart.specialList[index].Do();
        cooldowns[index].fillAmount = 0;

    }
    public void chooseAbility(int index)
    {
        abilityChoosePanel.SetActive(true);
        current = index;

    }
    public void changeAbility(int index)
    {
        GameStart.specialList[current] = specialAttack[index];
        selectionButtons[current].GetComponentInChildren<Text>().text = specialAttack[index].name;
        cooldowns.Add(GameStart.Instance.Buttons[current].transform.Find("Cooldown").GetComponent<Image>());
        abilityChoosePanel.SetActive(false);
    }
}
