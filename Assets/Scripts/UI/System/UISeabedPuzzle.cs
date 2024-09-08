using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISeabedPuzzle : UIBase
{
    public List<UIDragHandler> drag_list = new List<UIDragHandler>();
    public List<MouseEnterDetector> enter_list = new List<MouseEnterDetector>();
    private UIDragHandler curDrag = null;
    private MouseEnterDetector curEnter = null;
    private Vector3 dragPos;
    private bool[] ints = new bool[7] {false,false, false, false, false, false, false };
    private Transform rootTran;
    private int curNum = 0;
    public Button close_btn;
    public GameObject lockUI;
    public RectTransform bg;
    public Button rule_btn;
    // Start is called before the first frame update
    public override void OnStart()
    {
        for (int i = 0; i < drag_list.Count; i++)
        {
            drag_list[i].initialPos = drag_list[i].transform.position;
            drag_list[i].onDragEvent.AddListener(DragHandler);
            if (Screen.height < 1080)
            {
                drag_list[i].baseScale = 1 / ratio;

            }

        }
        close_btn.onClick.AddListener(CloseSelf);
        for (int i = 0; i < enter_list.Count; i++)
        {
            enter_list[i].onHover.AddListener(HoverHandler);
        }
        MissionManage.ShowDescription();
        rule_btn.onClick.AddListener(() =>
        {
        
            MissionManage.ShowDescription();

        });
    }
    private void HoverHandler(string state, MouseEnterDetector hover)
    {
        GameObject go = hover.gameObject;
        GuessingPuzzleNode curGrid = go.GetComponent<GuessingPuzzleNode>();

        if (state == "Enter")
        {
            curEnter = hover;
            Debug.Log("Enter");
        }
        else if (state == "Exit")
        {
            curEnter = null;
            Debug.Log("Exit");
            //   childTransform.gameObject.SetActive(false);

        }

    }
    private void DragHandler(string state, UIDragHandler uiDragHandler)
    {
        GameObject go = uiDragHandler.gameObject;
        Image image = go.transform.GetComponent<Image>();
        if (ints[uiDragHandler.itemId-1])
        {
            return;
        }
        if (state == "PointerDown")
        {
            dragPos = uiDragHandler.transform.localPosition;
            curDrag = uiDragHandler;
            rootTran = uiDragHandler.transform.parent;
            uiDragHandler.transform.SetParent(transform);
            image.raycastTarget = false;
            Debug.Log("Pointer Down");
        }
        else if (state == "Dragging")
        {
            // Debug.Log("Dragging");
        }
        else if (state == "PointerUp")
        {
            curDrag = null;
          
            uiDragHandler.transform.SetParent(rootTran);
            if (curEnter != null)
            {
                RectTransform rect = curEnter.transform.GetComponent<RectTransform>();
                go.transform.SetParent(rect);
                go.transform.localPosition = Vector3.zero;
                if (curEnter.itemId == uiDragHandler.itemId)
                {
                    ints[curEnter.itemId - 1] = true;
                    if (ints.Length > curEnter.itemId && ints[curEnter.itemId])
                    {
                        Transform line = curEnter.transform.Find("line");
                        line.gameObject.SetActive(true);

                    }
                    if (curEnter.itemId - 2 > -1 && ints[curEnter.itemId - 2])
                    {
                        Transform line = enter_list[curEnter.itemId - 2].transform.Find("line");
                        line.gameObject.SetActive(true);
                    }
                    DragonBonesController bones = UiManager.LoadBonesByNmae("seabed_puzzle_guang_bones");
                    bones.transform.SetParent(rect.parent);
                    bones.transform.localPosition = Vector3.zero;
                    bones.transform.localScale = Vector3.one;
                    bones.transform.SetAsFirstSibling();
                    curNum++;
                    if (curNum == 7)
                    {
                        DragonBonesController bones1 = UiManager.LoadBonesByNmae("seabed_puzzle_fazhen_bones");

                        bones1.transform.SetParent(bg);
                        bones1.transform.localPosition = new Vector3(0,40,0) ;
                        bones1.transform.localScale = Vector3.one;
                        bones1.PlayAnimation("01_Idle", false, () =>
                        {
                            UIFeedingFishSettle gui = Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(1), () => { }
                            , () => { CloseSelf(); }, () =>
                            {
                            

                            });
                            gui.come_back_btn.gameObject.SetActive(false);
                        });
                        lockUI.SetActive(true);




                    }
                }
                else
                {
                    image.raycastTarget = true;
                }
            }
            else
            {
                uiDragHandler.transform.localPosition = dragPos;
                image.raycastTarget = true;
            }
            Debug.Log("Pointer Up");
        }
    }
}
