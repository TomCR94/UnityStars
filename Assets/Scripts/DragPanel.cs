using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IDropHandler {

    public bool dockable = true;

    public RectTransform parent;
    public RectTransform sidePanel;
    public RectTransform BottomPanel;
    public RectTransform closedPanel;

    private Vector2 originalLocalPointerPosition;
	private Vector3 originalPanelLocalPosition;
	private RectTransform panelRectTransform;
	private RectTransform parentRectTransform;
	
	void Awake () {
		panelRectTransform = transform.parent as RectTransform;
		parentRectTransform = panelRectTransform.parent as RectTransform;
	}
	
	public void OnPointerDown (PointerEventData data) {
		originalPanelLocalPosition = panelRectTransform.localPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
	}
	
	public void OnDrag (PointerEventData data) {
		if (panelRectTransform == null || parentRectTransform == null)
			return;
		
		Vector2 localPointerPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (parentRectTransform, data.position, data.pressEventCamera, out localPointerPosition)) {
			Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
			panelRectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
		}
        transform.parent.SetAsLastSibling();
		//ClampToWindow ();
	}
	
	// Clamp panel to area of parent
	void ClampToWindow () {
		Vector3 pos = panelRectTransform.localPosition;
		
		Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
		Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;
		
		pos.x = Mathf.Clamp (panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
		pos.y = Mathf.Clamp (panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);
		
		panelRectTransform.localPosition = pos;
	}

    public void OnDrop(PointerEventData eventData)
    {
        if (!dockable)
            return;

        if (RectTransformUtility.RectangleContainsScreenPoint(sidePanel.GetComponent<RectTransform>(), eventData.position))
        {
            transform.parent.SetParent(sidePanel.GetComponentInChildren<ContentSizeFitter>().transform);
            enabled = false;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(BottomPanel.GetComponent<RectTransform>(), eventData.position))
        {
            transform.parent.SetParent(BottomPanel.GetComponentInChildren<ContentSizeFitter>().transform);
            enabled = false;
        }
        else
            transform.parent.SetParent(parent);

    }

    public void close()
    {
        transform.parent.SetParent(closedPanel);
    }

    public void moveToParent()
    {
        if (transform.parent == parent)
            return;
        transform.parent.SetParent(parent);
        transform.parent.SetAsLastSibling();
        transform.parent.position = new Vector2(Screen.width/2 + transform.parent.GetSiblingIndex() * 10, Screen.height/ 2 + transform.parent.GetSiblingIndex() * 10);
    }
    
}
