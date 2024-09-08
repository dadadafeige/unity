using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [System.Serializable]
    public class DragEvent : UnityEvent<string, UIDragHandler> { }

    public DragEvent onDragEvent;

    private RectTransform rectTransform;
    public int itemId = -1; 
    public Vector3 initialPos;
    public float baseScale = 1;
    public bool isCanMove = true;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (!isCanMove)
        {
            return;
        }
        // 在这里执行鼠标按下时的操作
        TriggerEvent("PointerDown");

        // 获取鼠标位置，并将 GameObject 的位置设置为鼠标位置
        Vector3 mousePosition = UiManager.uiCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // 设置 z 轴为 0，以确保在屏幕上正确显示
        transform.position = mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (!isCanMove)
        {
            return;
        }
        // 在这里执行拖拽时的操作
        TriggerEvent("Dragging");
        Vector2 fl = (baseScale - 1) * eventData.delta + eventData.delta;
        rectTransform.anchoredPosition += (eventData.delta * baseScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (!isCanMove)
        {
            return;
        }
        // 在这里执行鼠标释放时的操作
        TriggerEvent("PointerUp");
    }

    private void TriggerEvent(string eventName)
    {
        // 触发带有参数的 UnityEvent
        if (onDragEvent != null)
        {
            onDragEvent.Invoke(eventName, this);
        }
    }
}
