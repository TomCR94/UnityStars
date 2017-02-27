using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    float startSize = 1;
    [SerializeField]
    float minSize = 0.5f;
    [SerializeField]
    float maxSize = 1;

    [SerializeField]
    private float zoomRate = 5;
    
    private bool onObj = false;
    private void Update()
    {
        float scrollWheel = -Input.GetAxis("Mouse ScrollWheel");

        if (onObj && scrollWheel != 0)
        {
            ChangeZoom(scrollWheel);
        }

        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            ChangeZoom(deltaMagnitudeDiff); 
        }

        }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onObj = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onObj = false;
    }

    public void OnDisable()
    {
        onObj = false;
    }
    private void ChangeZoom(float scrollWheel)
    {
        float rate = 1 + zoomRate * Time.unscaledDeltaTime;
        if (scrollWheel > 0)
        {
            SetZoom(Mathf.Clamp(transform.localScale.y / rate, minSize, maxSize));
        }
        else
        {
            SetZoom(Mathf.Clamp(transform.localScale.y * rate, minSize, maxSize));
        }
    }

    public void SetZoom(float targetSize)
    {
        transform.localScale = new Vector3(targetSize, targetSize, 1);
    }

    public void AddZoom()
    {
        SetZoom(Mathf.Clamp(transform.localScale.y * 1.5f, minSize, maxSize));
    }

    public void SubZoom()
    {
        SetZoom(Mathf.Clamp(transform.localScale.y * 0.5f, minSize, maxSize));
    }
}
