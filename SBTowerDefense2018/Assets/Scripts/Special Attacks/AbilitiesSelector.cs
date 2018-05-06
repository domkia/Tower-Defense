using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesSelector : Singleton<AbilitiesSelector>
{
    public List<SpecialAttack> allAbilities;

    public int amountToSelect = 3;
    public GridLayoutGroup availableAbilitiesContainer;
    public GridLayoutGroup selectedAbilitiesContainer;

    public GameObject abilityUIPrefab;

    private void Start()
    {
        InstantiateUIElements();
    }

    public void Click(AbilityUIElement uiElement)
    {
        if (uiElement.transform.parent == availableAbilitiesContainer.transform 
            && selectedAbilitiesContainer.transform.childCount < amountToSelect)
            MoveTo(uiElement, selectedAbilitiesContainer);
        else if (uiElement.transform.parent == selectedAbilitiesContainer.transform)
            MoveTo(uiElement, availableAbilitiesContainer);
    }

    public void DragOnto(AbilityUIElement A, GameObject onto)
    {
        AbilityUIElement B = onto.GetComponent<AbilityUIElement>();
        if (B != null)
        {
            if (A.transform.parent == selectedAbilitiesContainer.transform && B.transform.parent == selectedAbilitiesContainer.transform)
                Swap(A, B);
            else if (A.transform.parent == availableAbilitiesContainer.transform && B.transform.parent == selectedAbilitiesContainer.transform)
            {
                int insertIndex = B.transform.GetSiblingIndex();
                MoveTo(A, B.transform.parent.GetComponent<GridLayoutGroup>(), insertIndex);
            }
            else
                MoveTo(A, B.transform.parent.GetComponent<GridLayoutGroup>());
            return;
        }
        GridLayoutGroup grid = onto.GetComponent<GridLayoutGroup>();
        if (grid != null)
            MoveTo(A, grid);
    }

    private void Swap(AbilityUIElement A, AbilityUIElement B)
    {
        int tempIndex = A.transform.GetSiblingIndex();
        A.transform.SetSiblingIndex(B.transform.GetSiblingIndex());
        B.transform.SetSiblingIndex(tempIndex);
    }

    private void MoveTo(AbilityUIElement A, GridLayoutGroup parent, int index = -1)
    {
        if (A.transform.parent == parent.transform)
        {
            A.Reset();
            return;
        }
        A.transform.SetParent(parent.transform);
        if (index != -1)
            A.transform.SetSiblingIndex(index);
    }

    private void InstantiateUIElements()
    {
        for (int i = 0; i < allAbilities.Count; i++)
        {
            AbilityUIElement element = Instantiate(abilityUIPrefab, availableAbilitiesContainer.transform).GetComponentInChildren<AbilityUIElement>();
            element.Setup(allAbilities[i]);
        }
    }
}