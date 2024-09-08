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
        // ������ִ����갴��ʱ�Ĳ���
        TriggerEvent("PointerDown");

        // ��ȡ���λ�ã����� GameObject ��λ������Ϊ���λ��
        Vector3 mousePosition = UiManager.uiCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // ���� z ��Ϊ 0����ȷ������Ļ����ȷ��ʾ
        transform.position = mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (!isCanMove)
        {
            return;
        }
        // ������ִ����קʱ�Ĳ���
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
        // ������ִ������ͷ�ʱ�Ĳ���
        TriggerEvent("PointerUp");
    }

    private void TriggerEvent(string eventName)
    {
        // �������в����� UnityEvent
        if (onDragEvent != null)
        {
            onDragEvent.Invoke(eventName, this);
        }
    }
}
