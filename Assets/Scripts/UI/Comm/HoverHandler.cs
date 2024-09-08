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
        // �����봥�����������Ƿ���ָ���Ĳ㼶��
        onHover.Invoke("Enter", this);
        // ������ִ�д�����������ʱ�Ĳ���
        // ���磺���ٻ����ش��������ڵ�����
        // gameObject.SetActive(false);

    }
    void OnTriggerExit(Collider other)
    {
        onHover.Invoke("Exit", this);
      
        // ������ִ���˳�������ʱ�Ĳ���


    }
    void OnTriggerStay(Collider other)
    {
        onHover.Invoke("OnTrigger", this);
       
            // ������ִ�д������г������еĲ���
       
    }
 
}
//public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
//{
//    [System.Serializable]
//    public class HoverEvent : UnityEvent<string,GameObject> { }

//    public HoverEvent onHover;

//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        // ������ʱ�Ĵ���

//        onHover.Invoke("Enter", gameObject); // ���ý����¼��������ַ������� "Enter"
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        // ����˳�ʱ�Ĵ���

//        onHover.Invoke("Exit", gameObject); // �����˳��¼��������ַ������� "Exit"
//    }


//}
