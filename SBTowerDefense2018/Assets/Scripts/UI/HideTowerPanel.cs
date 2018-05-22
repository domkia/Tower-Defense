using UnityEngine;

public class HideTowerPanel : MonoBehaviour
{
    public bool IsOpen { get; private set; }
    public RectTransform towerPanel;
    public float margin = 50f;
    Vector2 mousePos;

    private void Start()
    {
        hidePanel();
    }

    public void hidePanel()
    {
        towerPanel.gameObject.SetActive(false);
        IsOpen = false;
    }

    public void showPanel()
    {
        towerPanel.gameObject.SetActive(true);
        IsOpen = true;
    }

    private void LateUpdate()
    {
        mousePos = Input.mousePosition;
        if (!IsOpen)
        {
            if (mousePos.x >= Screen.width - margin)
                showPanel();
        }
        else
        {
            bool inside = RectTransformUtility.RectangleContainsScreenPoint(towerPanel, mousePos);
            if (inside == false)
                hidePanel();
        }
    }
}
