using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumbeRmemoryItemNode : MonoBehaviour
{
    public class ClickEvent : UnityEvent<NumbeRmemoryItemNode> { }
    Button btn;
    public ClickEvent clickEvent = new ClickEvent();
    public int gridI;
    public int gridZ;
    public bool isClick = false;
    public int gridType = 0;
    public TextMeshProUGUI number;
    public GameObject pop;
    public RectTransform bones_root;
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            
            clickEvent.Invoke(this);

        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
