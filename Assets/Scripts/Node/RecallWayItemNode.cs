using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecallWayItemNode : MonoBehaviour
{
    public class ClickEvent : UnityEvent<RecallWayItemNode> { }
    Button btn;
    public ClickEvent clickEvent = new ClickEvent();
    public int gridI;
    public int gridZ;
    public bool isClick = false;
    public RectTransform bones_root;
    public Image yun;
    public int gridType = 0;
    public DragonBonesController ball_bomb_bones = null;
    public DragonBonesController dragon;
    public GameObject can_click;
    public int YunType = -1;
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
