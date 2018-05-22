using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilitiesSelector : Singleton<AbilitiesSelector>
{
    public Button startButton;
    public List<SpecialAttack> allAbilities;

    public int amountToSelect = 3;
    public Transform availableAbilitiesContainer;
    public Transform selectedAbilitiesContainer;

    public GameObject abilityUISlotPrefab;
    private GameObject tempUISlot;

    private void Start()
    {
        InstantiateSlots();
        SetupUIElements();
        ShowHideStartButton();

        UnityAction startButtonCallback = new UnityAction(()=>
        {
            AbilitiesManager.Instance.StartGame(GetSelectedAbilities());
            startButton.gameObject.SetActive(false);
            GameManager.OnAbilitiesSelected();
        });
        startButton.onClick.AddListener(startButtonCallback);
    }

    private List<AbilityUIElement> GetSelectedAbilities()
    {
        List<AbilityUIElement> a = new List<AbilityUIElement>();
        for (int i = 0; i < selectedAbilitiesContainer.childCount; i++)
            a.Add(selectedAbilitiesContainer.GetChild(i).GetComponent<AbilityUIElement>());
        return a;
    }

    public void Move(AbilityUIElement abilityUI)
    {
        AbilityUIElement newSlot = null;
        if (abilityUI.transform.parent == availableAbilitiesContainer && NotEmptyAmount(selectedAbilitiesContainer) < amountToSelect)
            newSlot = GetEmptySlot(selectedAbilitiesContainer);
        else if (abilityUI.transform.parent == selectedAbilitiesContainer)
            newSlot = GetEmptySlot(availableAbilitiesContainer);
        if (newSlot == null)
            Debug.Log("Can't find empty slot");
        newSlot.Setup(abilityUI.ability);
        abilityUI.Reset();
        ShowHideStartButton();
    }

    public void BeginDrag(AbilityUIElement abilityUIElement)
    {
        int index = abilityUIElement.transform.GetSiblingIndex();

        Transform parent = abilityUIElement.transform.parent;
        abilityUIElement.transform.SetParent(transform, false);
        tempUISlot.transform.SetParent(parent, false);

        tempUISlot.transform.SetSiblingIndex(index);
        tempUISlot.SetActive(true);
    }

    public void EndDrag(AbilityUIElement abilityUIElement, GameObject onto)
    {
        if (onto == null)
        {
            //Debug.Log("EndDrag onto nothing");
            CancelDrag(abilityUIElement);
            return;
        }
        if (onto.GetComponent<AbilityUIElement>() != null)
        {
            if (onto == tempUISlot)
            {
                //Debug.Log("Same object");
                CancelDrag(abilityUIElement);
                return;
            }

            //Debug.Log("EndDrag onto another element");
            AbilityUIElement other = onto.GetComponent<AbilityUIElement>();
            CancelDrag(abilityUIElement);
            if (other.IsEmpty)
            {
                other.Setup(abilityUIElement.ability);
                abilityUIElement.Reset();
            }
            else
            {
                SpecialAttack temp = other.ability;
                other.Setup(abilityUIElement.ability);
                abilityUIElement.Setup(temp);
            }
        }
        else if (onto.transform == availableAbilitiesContainer 
            || onto.transform == selectedAbilitiesContainer)
        {
            Debug.Log("EndDrag onto container");
            CancelDrag(abilityUIElement);
            GetEmptySlot(onto.transform).Setup(abilityUIElement.ability);
            abilityUIElement.Reset();
        }
        else
            CancelDrag(abilityUIElement);
        ShowHideStartButton();
    }

    private void CancelDrag(AbilityUIElement abilityUIElement)
    {
        int index = tempUISlot.transform.GetSiblingIndex();
        Transform parent = tempUISlot.transform.parent;
        abilityUIElement.transform.SetParent(parent, false);
        abilityUIElement.transform.SetSiblingIndex(index);

        tempUISlot.transform.SetParent(transform);
        tempUISlot.SetActive(false);
    }

    private AbilityUIElement GetEmptySlot(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            AbilityUIElement ui = parent.GetChild(i).GetComponent<AbilityUIElement>();
            if (ui.IsEmpty)
                return ui;
        }
        Debug.LogError("There is no empty slot");
        return null;
    }

    private int NotEmptyAmount(Transform parent)
    {
        int amount = 0;
        for (int i = 0; i < parent.childCount; i++)
            if (parent.GetChild(i).GetComponent<AbilityUIElement>().IsEmpty == false)
                amount++;
        return amount;
    }

    private void InstantiateSlots()
    {
        //Instantiate available slots
        GridLayoutGroup group = availableAbilitiesContainer.GetComponent<GridLayoutGroup>();
        int availableCount = allAbilities.Count + allAbilities.Count % group.constraintCount;
        for (int i = 0; i < availableCount; i++)
            Instantiate(abilityUISlotPrefab, availableAbilitiesContainer);

        //Instantiate selected slots
        for (int i = 0; i < amountToSelect; i++)
            Instantiate(abilityUISlotPrefab, selectedAbilitiesContainer);

        //Instantiate temp
        tempUISlot = Instantiate(abilityUISlotPrefab, transform);
        tempUISlot.GetComponent<Image>().color = Color.gray;
        tempUISlot.gameObject.SetActive(false);
    }

    private void SetupUIElements()
    {
        //Setup all available abilities
        for (int i = 0; i < allAbilities.Count; i++)
        {
            Transform slot = availableAbilitiesContainer.GetChild(i);
            AbilityUIElement element = slot.GetComponentInChildren<AbilityUIElement>();
            element.Setup(allAbilities[i]);
        }
    }

    void ShowHideStartButton()
    {
        int count = NotEmptyAmount(selectedAbilitiesContainer);
        bool inter = count == amountToSelect ? true : false;
        CanvasGroup group = startButton.GetComponent<CanvasGroup>();
        if (inter)
        {
            group.alpha = 1f;
            group.interactable = true;
        }
        else
        {
            group.alpha = 0.5f;
            group.interactable = false;
        }
    }
}