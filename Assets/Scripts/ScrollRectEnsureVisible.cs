using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectEnsureVisible : MonoBehaviour
{

    public float _AnimTime = 0.15f;
    public bool _Snap = false;
    public bool _LockX = false;
    public bool _LockY = true;
    public RectTransform _MaskTransform;
    
    private ScrollRect mScrollRect;
    private Transform mScrollTransform;
    private RectTransform mContent;
    
    private void Awake()
    {
        mScrollRect = GetComponent<ScrollRect>();
        mScrollTransform = mScrollRect.transform;
        mContent = mScrollRect.content;
    }
    
    public void CenterOnItem(RectTransform target)
    {
        Vector3 maskCenterPos = _MaskTransform.position + (Vector3)_MaskTransform.rect.center;
        Debug.Log("Mask Center Pos: " + maskCenterPos);
        Vector3 itemCenterPos = target.position;
        Debug.Log("Item Center Pos: " + itemCenterPos);
        Vector3 difference = maskCenterPos - itemCenterPos;
        difference.z = 0;

        Vector3 newPos = mContent.position + difference;
        if (_LockX)
        {
            newPos.x = mContent.position.x;
        }
        if (_LockY)
        {
            newPos.y = mContent.position.y;
        }

        if (_Snap)
        {
            mContent.position = newPos;
        }
        else
        {
            DOTween.To(() => mContent.position, x => mContent.position = x, newPos, _AnimTime);
        }
    }
}