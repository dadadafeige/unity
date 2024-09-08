using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HoverHandler : MonoBehaviour
{
    [System.Serializable]
    public class HoverEvent : UnityEvent<string, HoverHandler> { }
    private BoxCollider boxCollider;
    public HoverEvent onHover;
    public GameObject targetObj;
    private void Start()
    {
        boxCollider = gameObject.transform.GetComponent<BoxCollider>();
    }
    void OnTriggerEnter(Collider other)
    {
        // 检测进入触发器的物体是否在指定的层级中
        onHover.Invoke("Enter", this);
        // 在这里执行触发器被进入时的操作
        // 例如：销毁或隐藏触发器所在的物体
        // gameObject.SetActive(false);

    }
    void OnTriggerExit(Collider other)
    {
        onHover.Invoke("Exit", this);
      
        // 在这里执行退出触发器时的操作


    }
    void OnTriggerStay(Collider other)
    {
        onHover.Invoke("OnTrigger", this);
       
            // 在这里执行触发器中持续进行的操作
       
    }
 
}
//public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
//{
//    [System.Serializable]
//    public class HoverEvent : UnityEvent<string,GameObject> { }

//    public HoverEvent onHover;

//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        // 鼠标进入时的处理

//        onHover.Invoke("Enter", gameObject); // 调用进入事件，传递字符串参数 "Enter"
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        // 鼠标退出时的处理

//        onHover.Invoke("Exit", gameObject); // 调用退出事件，传递字符串参数 "Exit"
//    }


//}
