using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : Singleton<InputController>, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Events
    public static event Action<Vector3> PointerDown;
    public static event Action<Vector3> PointerUp;

    public static event Action<Vector3> DragBegin;
    public static event Action<Vector3> Dragging;
    public static event Action<PointerEventData> DraggingEventData;
    public static event Action<Vector3> DragEnd;
    #endregion

    [SerializeField] private Image _raycastCatcher;
    [SerializeField] private bool _multiTouchEnabled = false;

    private bool isActive = true;
    private GraphicRaycaster _graphicRaycaster;

    protected override void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _graphicRaycaster.enabled = true;
        Input.multiTouchEnabled = _multiTouchEnabled;
    }

    private void OnDisable()
    {
    }

    private void OnLevelCompleted(bool obj)
    {
        _graphicRaycaster.enabled = false;
    }

    private void OnLevelLoaded()
    {
        DOVirtual.DelayedCall(0.1f, () => _graphicRaycaster.enabled = true);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isActive)
            DragBegin?.Invoke(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            Dragging?.Invoke(eventData.position);
            DraggingEventData?.Invoke(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isActive)
            DragEnd?.Invoke(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isActive)
            PointerDown?.Invoke(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isActive)
            PointerUp?.Invoke(eventData.position);
    }


    public void Toggle(bool on)
    {
        isActive = on;
    }
}