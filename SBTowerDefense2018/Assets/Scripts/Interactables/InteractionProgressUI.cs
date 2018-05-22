using UnityEngine;
using UnityEngine.UI;

public class InteractionProgressUI : MonoBehaviour
{
    public Image progressFill;                  //Image with fillMode: 360, origin: top

    private void Start()
    {
        GameManager.OnGameOver += () => gameObject.SetActive(false);
    }

    public void SetProgress(float progress)
    {
        progressFill.fillAmount = progress;
    }

    public void Reset()
    {
        progressFill.fillAmount = 0f;
    }
}
