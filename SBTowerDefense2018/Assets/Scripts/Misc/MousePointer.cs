using UnityEngine;

/// <summary>
/// Temporary script to visualize when the mouse is clicked
/// **Not gonna make into the final build.
/// </summary>
public class MousePointer : MonoBehaviour
{
    [SerializeField] private Texture2D normal;
    [SerializeField] private Texture2D click;
    private Vector2 _pivot;

	void Start ()
    {
        SetCursor(normal);
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
            SetCursor(click);
        else if (Input.GetMouseButtonUp(0))
            SetCursor(normal);
	}

    private void SetCursor(Texture2D tex)
    {
        Cursor.SetCursor(tex, _pivot, CursorMode.ForceSoftware);
    }
}
