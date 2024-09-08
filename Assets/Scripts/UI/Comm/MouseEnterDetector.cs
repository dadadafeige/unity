using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using static HoverHandler;

public class MouseEnterDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [System.Serializable]
    public class OnMenterEnter : UnityEvent<string, MouseEnterDetector> { }
    public GameObject targetObj;
    public OnMenterEnter onHover;
    public int itemId = -1;
    // ��������ͼ��Χʱ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover.Invoke("Enter", this);
        Debug.Log("Mouse entered image range.");
        // �����������ϣ��ִ�е��κβ�����������ʾ��ʾ��Ϣ�򴥷������¼���
    }

    // ������뿪ͼ��Χʱ����
    public void OnPointerExit(PointerEventData eventData)
    {
        onHover.Invoke("Exit", this);
        Debug.Log("Mouse exited image range.");
        // �����������ϣ��ִ�е��κβ���������������ʾ��Ϣ�����������¼�״̬��
    }
}
