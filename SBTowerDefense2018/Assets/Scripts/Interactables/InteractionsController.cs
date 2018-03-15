using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionsController : MonoBehaviour
{
    public LayerMask interactionLayer;              //Layer for raycasting interactables
    public InteractionProgressUI progressUI;        //Radial progress bar UI

    private IInteractable currentInteractable;

	void Start ()
    {
        HideProgressBar();                          //At start, hide the progress bar
	}

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())      //return if over UI element
            return;

        if (Input.GetMouseButtonDown(0))                        //Called once
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f, interactionLayer))
            {
                IInteractable inter = hit.collider.GetComponent<IInteractable>();
                if (inter != null)                              
                {
                    currentInteractable = inter;
                    inter.OnCompleted += RemoveInteractable;    //We need to set currentInteractable to null when interaction has completed
                    progressUI.GetComponent<RectTransform>().position = Input.mousePosition;    //Set progress UI position to where the touch is
                    progressUI.gameObject.SetActive(true);                                      //Show progress UI
                }
            }
        }

        if (currentInteractable == null)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            currentInteractable.Cancel();
            currentInteractable = null;
            HideProgressBar();
        }

        if (currentInteractable == null)
            return;

        if (Input.GetMouseButton(0))
        {
            float progress = currentInteractable.UpdateProgress();
            progressUI.SetProgress(progress);           //Update progress UI
        }
    }

    void RemoveInteractable(IInteractable inter)
    {
        HideProgressBar();
        currentInteractable = null;
        inter.OnCompleted -= RemoveInteractable;
    }

    void HideProgressBar()
    {
        progressUI.Reset();
        progressUI.gameObject.SetActive(false);
    }
}
