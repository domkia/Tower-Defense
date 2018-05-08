using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilitiesManager : Singleton<AbilitiesManager>
{
    private List<AbilityUIElement> specialAttacks = new List<AbilityUIElement>();

    public GameObject abilityChoosePanel;
    public GameObject selectedAbilitiesPanel;

    private bool gameStarted = false;

    public void StartGame(List<AbilityUIElement> abilities)
    {
        specialAttacks = abilities;
        SetupAbilitiesPannel();
        abilityChoosePanel.SetActive(false);
        AbilitiesSelector.Instance.enabled = false;
        
        selectedAbilitiesPanel.SetActive(true);
        gameStarted = true;
    }

    private void SetupAbilitiesPannel()
    {
        for (int i = 0; i < specialAttacks.Count; i++)
        {
            specialAttacks[i].cooldown.fillAmount = 1f;
            specialAttacks[i].enabled = false;
            Button button = specialAttacks[i].gameObject.AddComponent<Button>();
            Debug.Log(i);
            int a = i;
            button.onClick.AddListener(
                () => Do(a)
            );
            specialAttacks[i].transform.SetParent(selectedAbilitiesPanel.transform, false);
        }
    }

    private void Do(int index)
    {
        if (specialAttacks[index].ability.IsReady)
        {
            specialAttacks[index].ability.Do();
        }
    }
	
	void Update ()
    {
        if (gameStarted == false)
            return;
        for(int i = 0; i < specialAttacks.Count; i++)
        {
            specialAttacks[i].ability.UpdateCooldown();
            specialAttacks[i].cooldown.fillAmount = specialAttacks[i].ability.CooldownProgress;
        }
	}
}
