using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class AbilityUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpecialAttack ability = null;
    public bool IsEmpty { get { return ability == null; } }
    public Image cooldown;

    private void Awake()
    {
        Reset();
    }

    public void Setup(SpecialAttack specialAttack)
    {
        ability = specialAttack;
        transform.Find("Icon").GetComponent<Image>().sprite = ability.icon;
        transform.Find("Title").GetComponent<Text>().text = ability.title;
        cooldown = transform.Find("Cooldown").GetComponent<Image>();
    }

    public void Reset()
    {
        ability = null;
        transform.Find("Icon").GetComponent<Image>().sprite = null;
        transform.Find("Title").GetComponent<Text>().text = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        GetComponent<Image>().raycastTarget = false;
        AbilitiesSelector.Instance.BeginDrag(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        GameObject onto = eventData.pointerCurrentRaycast.gameObject;
        AbilitiesSelector.Instance.EndDrag(this, onto);
        GetComponent<Image>().raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        transform.position = Input.mousePosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        if (eventData.delta.magnitude < 5f)
            AbilitiesSelector.Instance.Move(this);
    }

    Tweener scaleIn;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        scaleIn = transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        scaleIn.Kill();
        transform.localScale = Vector3.one;
    }
}
