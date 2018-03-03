using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is PC version of CameraController. No smoothing on this one because
/// we wouldn't want 'floaty' controls on mobile.
/// TODO: Mobile version -- instead of using mouse wheel to zoom, use 2 fingers to pinch the screen
/// </summary>
[RequireComponent(typeof(Camera))]
public class PCCameraController : MonoBehaviour
{
    [SerializeField] private Bounds bounds;                             //Scene boundaries
    [SerializeField] private float zoomMultiplier = 1f;                 //Mouse wheel sensitivity
    [SerializeField] [Range(45f, 90f)] private float angleLow = 45f;    //Camera angle based on height
    [SerializeField] [Range(45f, 90f)] private float angleHigh = 90f;   
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;

    private Plane _groundPlane;
    private Transform _trans;
    private Camera _cam;
    private Vector3 _touchStartPos;
    private Vector3 _viewPortCenter;
    private bool _dragging = false;
    private float _currAngle;

	void Start ()
    {
        _trans = GetComponent<Transform>();
        _cam = GetComponent<Camera>();
        _groundPlane = new Plane(Vector3.up, Vector3.zero);     //NOTE: it's an imaginary plane on which we cast the ray.
                                                                // make sure our grid is leveled correctly on the Y axis.
        
        //Set camera's initial position and rotation
        float startingZoom = (minZoom + maxZoom) * 0.5f;
        _currAngle = GetAngle(startingZoom);
        _trans.rotation = Quaternion.Euler(_trans.right * _currAngle);
        _trans.position = new Vector3(_trans.position.x, startingZoom, _trans.position.z);

        _viewPortCenter = new Vector3(0.5f, 0.5f, 0f);          //Screen center
    }

    void Update ()
    {
        //If mouse cursor / touch is over UI element, don't move the camera
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 finalPosition = _trans.position;

        //Panning
        if (Input.GetMouseButton(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))            //Called once
            {
                _touchStartPos = GetPointOnPlane(ray);
                _dragging = true;
            }
            if (_dragging)
            {
                Vector3 newTouchPos = GetPointOnPlane(ray);
                finalPosition += _touchStartPos - newTouchPos;
                
                //Make sure camera stays in bounds
                finalPosition.x = Mathf.Clamp(finalPosition.x, -bounds.extents.x / 2f, bounds.extents.x / 2f);
                finalPosition.z = Mathf.Clamp(finalPosition.z, -bounds.extents.z / 2f, bounds.extents.z / 2f);
            }
        }
        else
            _dragging = false;

        //Zooming
        float scrollAmount = Input.mouseScrollDelta.y;
        if (scrollAmount != 0f)
        {
            //Cast a ray from the center of screen. Get distance to the ground
            Ray centerRay = _cam.ViewportPointToRay(_viewPortCenter);
            float currZoom;
            _groundPlane.Raycast(centerRay, out currZoom);
            Vector3 pivotPoint = centerRay.origin + centerRay.direction * currZoom;

            currZoom -= scrollAmount * zoomMultiplier;
            currZoom = Mathf.Clamp(currZoom, minZoom, maxZoom);

            //Compensate for camera angle
            _currAngle = GetAngle(currZoom);
            float z = Mathf.Cos(_currAngle * Mathf.Deg2Rad) * currZoom;
            float y = Mathf.Tan(_currAngle * Mathf.Deg2Rad) * z;
            finalPosition.y = y;
            finalPosition.z = pivotPoint.z - z;
        }

        //Set rotation
        Vector3 rot = _trans.eulerAngles;
        rot.x = _currAngle;
        _trans.eulerAngles = rot;

        //Set final position
        _trans.position = finalPosition;          
    }

    private Vector3 GetPointOnPlane(Ray ray)
    {
        float hitDistance;
        _groundPlane.Raycast(ray, out hitDistance);
        return ray.GetPoint(hitDistance);
    }

    private float GetAngle(float currentZoom)
    {
        float angleFactor = (currentZoom - minZoom) / (maxZoom - minZoom);
        float angle = Mathf.Lerp(angleLow, angleHigh, angleFactor);
        return angle;
    }

    private void OnDrawGizmos()
    {
        //Visualize boundaries
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.extents);
    }
}
