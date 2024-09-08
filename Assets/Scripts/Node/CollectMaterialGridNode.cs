using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollectMaterialGridNode : MonoBehaviour
{
    public class ClickEvent : UnityEvent<CollectMaterialGridNode> { }
    Button btn;
    public ClickEvent clickEvent = new ClickEvent();
    public int gridType = 0; //0 �ո���  1 ����
    public int gridI;
    public int gridZ;

    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                clickEvent.Invoke(this);

            });
        }
       
    }
    
}
