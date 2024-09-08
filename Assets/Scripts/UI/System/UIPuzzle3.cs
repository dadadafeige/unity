using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPuzzle3 : UIBase
{
    public List<HoverHandler> hovers = new List<HoverHandler>();
    public List<UIDragHandler> uIDrags = new List<UIDragHandler>();
    public Button closeBtn;
    public GameObject enterObj;
    public GameObject showMask;
    private List<GameObject> intersectings = new List<GameObject>();
    private GameObject curDragObj;
    private GameObject curShowObj;
    private int rightNum = 0;
    Action callBack;
    public Button rule_btn;
    private bool isOneOk = false;
    public DragonBonesController dragon;
    public override void OnAwake()
    {

    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        MissionManage.ShowDescription();

        closeBtn.onClick.AddListener(CloseSelf);
        InitUI();
        rule_btn.onClick.AddListener(() =>
        {

            MissionManage.ShowDescription();

        });

    }
    public void SetCallBack(Action callBack)
    {
        this.callBack = callBack;



    }
    //private void Update()
    //{
    //    if (curDragObj != null && intersectings.Count > 0)
    //    {
    //        GameObject go = CalculateLargestContactArea();
    //        if (go != null && go != curShowObj)
    //        {
    //            if (curShowObj != null)
    //            {
    //                curShowObj.SetActive(false);
    //            }
    //            GameObject childTransform = go.transform.GetChild(0).gameObject;
    //            childTransform.SetActive(true);
    //            curShowObj = childTransform;
    //        }
       
    //    }
    //}
    private void InitUI()
    {
        for (int i = 0; i < hovers.Count; i++)
        {
            hovers[i].onHover.AddListener(HoverHandler);
        }
        for (int i = 1; i < uIDrags.Count; i++)
        {
            uIDrags[i].GetComponent<Image>().raycastTarget = false;
        }

        uIDrags[0].initialPos = uIDrags[0].transform.position;
        uIDrags[0].onDragEvent.AddListener(DragHandler);

    }
    private void HoverHandler(string state, HoverHandler hover)
    {
        GameObject go = hover.gameObject;
        if (hover.targetObj != curDragObj)
        {
            return;
        }
        UIDragHandler uIDrag = curDragObj.GetComponent<UIDragHandler>();
        Transform childTransform = go.transform.GetChild(0);
        if (state == "Enter")
        {
            intersectings.Add(go);
            childTransform.gameObject.SetActive(true);
            curShowObj = childTransform.gameObject;
            Debug.Log("Mouse entered!");
        }
        else if (state == "Exit")
        {
            intersectings.Remove(go);
          
            if (curShowObj != null && uIDrag.itemId == -1)
            {
                curShowObj.SetActive(false);
                curShowObj = null;
            }
         //   childTransform.gameObject.SetActive(false);
            Debug.Log("Mouse exited!");
        }

    }
    private GameObject currHover;
        

    private void DragHandler(string state, UIDragHandler uiDragHandler)
    {
        if (!isOneOk && uiDragHandler.itemId == -1)
        {
            return;
        }
        GameObject go = uiDragHandler.gameObject;
        Image image = go.transform.GetComponent<Image>();
        if (state == "PointerDown")
        {
            for (int i = 0; i < hovers.Count; i++)
            {
                if (hovers[i].targetObj == uiDragHandler.gameObject)
                {
                    GameObject obj = hovers[i].transform.GetChild(0).gameObject;
                    currHover = obj;
;
                    obj.gameObject.SetActive(true);
                    break;

                }
            }
            
            image.transform.localScale = Vector3.one;
            curDragObj = go;

            image.raycastTarget = false;
            Debug.Log("Pointer Down");
        }
        else if (state == "Dragging")
        {
           // Debug.Log("Dragging");
        }
        else if (state == "PointerUp")
        {
       
            if (curShowObj != null)
            {
                rightNum++;
                curDragObj.transform.position = curShowObj.transform.position;
                if (!isOneOk)
                {
                    for (int i = 1; i < uIDrags.Count; i++)
                    {
                       
                        uIDrags[i].GetComponent<Image>().raycastTarget = true;
                        uIDrags[i].initialPos = uIDrags[i].transform.position;
                        uIDrags[i].onDragEvent.AddListener(DragHandler);
                    }
                    isOneOk = true;
                }
                if (rightNum == 5)
                {
                    dragon.gameObject.SetActive(true);
                    dragon.PlayAnimation("01_Idle", false, () =>
                    {
                        UIFeedingFishSettle gui = Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(1), () =>
                        {

                            CloseSelf();
                        }, () => {
                            if (callBack != null)
                            {
                                callBack.Invoke();

                            }

                            CloseSelf();
                        }, () =>
                        {


                        });
                        gui.come_back_btn.gameObject.SetActive(false);

                    });
                   

                }
                if (!isOneOk && uiDragHandler.itemId == -1)
                {
                    if (currHover != null)
                    {
                        currHover.SetActive(false);
                        currHover = null;
                    }
                }

            }
            else
            {
                image.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                image.raycastTarget = true;
                curDragObj.transform.position = uiDragHandler.initialPos;
                if (!isOneOk && uiDragHandler.itemId == -1)
                {
                    if (currHover != null)
                    {
                        currHover.SetActive(false);
                        currHover = null;
                    }
                }
               
            }
            curDragObj = null;
            curShowObj = null;
            currHover = null;
            Debug.Log("Pointer Up");
        }
    }
    float CalculateContactArea(Bounds bounds1, Bounds bounds2)
    {
        // 计算两个边界框的交叉区域
        float xOverlap = Mathf.Min(bounds1.max.x, bounds2.max.x) - Mathf.Max(bounds1.min.x, bounds2.min.x);
        float yOverlap = Mathf.Min(bounds1.max.y, bounds2.max.y) - Mathf.Max(bounds1.min.y, bounds2.min.y);
        float zOverlap = Mathf.Min(bounds1.max.z, bounds2.max.z) - Mathf.Max(bounds1.min.z, bounds2.min.z);

        // 判断是否有交叉，有则计算交叉面积
        if (xOverlap > 0 && yOverlap > 0 && zOverlap > 0)
        {
            return xOverlap * yOverlap * zOverlap;
        }
        else
        {
            // 没有交叉的话返回0
            return 0f;
        }
    }
    GameObject CalculateLargestContactArea()
    {
        if (curDragObj == null)
        {
            return null;
        }
        float largestContactArea = 0f;
        GameObject largest = null;
   
        BoxCollider boxCollider1 = curDragObj.transform.GetComponent<BoxCollider>();
        // 遍历相交的 Collider 列表
        foreach (GameObject collider in intersectings)
        {

            BoxCollider boxCollider2 = collider.transform.GetComponent<BoxCollider>();


            Bounds bounds1 = boxCollider1.bounds;
            Bounds bounds2 = boxCollider2.bounds;

            // 计算当前 Collider 的交叉面积
            float contactArea = CalculateContactArea(bounds1, bounds2);

            // 比较面积大小，更新最大面积和对应的 Collider
            if (contactArea > largestContactArea)
            {
                largestContactArea = contactArea;
                largest = collider;
            }
        }
        return largest;
        Debug.Log("Largest Contact Area: " + largestContactArea);
        // 在这里你可以使用 largestCollider 做进一步的处理
    }

}
