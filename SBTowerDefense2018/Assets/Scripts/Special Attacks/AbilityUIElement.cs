using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [HideInInspector]
    public SpecialAttack ability = null;

    private Vector2 snapPosition;

    public void Setup(SpecialAttack specialAttack)
    {
        ability = specialAttack;
        transform.Find("Icon").GetComponent<Image>().sprite = ability.icon;
        transform.Find("Title").GetComponent<Text>().text = ability.title;
        transform.Find("Cooldown").GetComponent<Image>().fillAmount = 0f;
    }

    public void Reset()
    {
        transform.position = snapPosition;
        GetComponent<Image>().raycastTarget = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = false;
        snapPosition = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject onto = eventData.pointerCurrentRaycast.gameObject;
        if (onto == null)
        {
            Reset();
            return;
        }
        AbilitiesSelector.Instance.DragOnto(this, onto);
        Reset();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.delta.magnitude == 0f)
            AbilitiesSelector.Instance.Click(this);
    }
}
