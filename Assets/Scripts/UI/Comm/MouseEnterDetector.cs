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
    // 当鼠标进入图像范围时调用
    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover.Invoke("Enter", this);
        Debug.Log("Mouse entered image range.");
        // 在这里添加您希望执行的任何操作，例如显示提示信息或触发其他事件。
    }

    // 当鼠标离开图像范围时调用
    public void OnPointerExit(PointerEventData eventData)
    {
        onHover.Invoke("Exit", this);
        Debug.Log("Mouse exited image range.");
        // 在这里添加您希望执行的任何操作，例如隐藏提示信息或重置其他事件状态。
    }
}
