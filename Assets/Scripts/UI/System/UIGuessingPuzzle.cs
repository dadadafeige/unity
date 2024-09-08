using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CustomTimer;

public class UIGuessingPuzzle : UIBase
{
    // Start is called before the first frame update
    public RectTransform grid_root1;
    public RectTransform grid_root2;
    public RectTransform grid_root3;
    private RectTransform grid_root;
    public GameObject mask;
    public Image time_pro;
    public GameObject grid;
    public List<UIDragHandler> bag_gemstone_list = new List<UIDragHandler>();
    private int rowNum;
    private int colNum;
    private List<GuessingPuzzleNode> gridNodeList = new List<GuessingPuzzleNode>();
    private GuessingPuzzleNode[,] grids;
    private List<GameObject> gemstoneList = new List<GameObject>();
    private List<GameObject> curGemstoneList = new List<GameObject>();
    public List<TextMeshProUGUI> gemstoneNumList = new List<TextMeshProUGUI>();
    private List<int> gemstoneId = new List<int>();
    private GameObject curDragObj;
    private int curDragId = -1;
    private GuessingPuzzleNode enterGrid;
    public GameObject gemstone_item;
    private Dictionary<int, int> consumeMap = new Dictionary<int, int>();
    private int diff = 0;
    private List<GuessingPuzzleNode> curGridList = new List<GuessingPuzzleNode>();
    public RawImage diff_bg;
    private int curRightNum = 0;
    public TextMeshProUGUI mTick;
    Tween tween;
    public Button close_btn;
    public List<GameObject> ditt_itme_list;
    private TimerInfo timerInfo;
    public Button rule_btn;

    //  private 
    public override void OnStart()
    {
        InitUI();
        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                UpdataDiff();
            });
        });
     
        close_btn.onClick.AddListener(CloseSelf);
        rule_btn.onClick.AddListener(() =>
        {
            if (tween != null)
            {
                tween.Pause();

            }
            MissionManage.ShowDescription(() =>
            {
                if (tween != null)
                {
                    tween.Play();

                }

            });



        });

    }
    private void InitGrid()
    {
        for (int i = 0; i < curGridList.Count; i++)
        {
            PushGrid(curGridList[i]); 
        }
        for (int i = 0; i < colNum; i++)
        {
            for (int z = 0; z < rowNum; z++)
            {
                GuessingPuzzleNode node = PopGrid();
                grids[z, i] = node;
                UpdatGrid(node);
                node.gridZ = z;
                node.gridI = i;
                curGridList.Add(node);

            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(grid_root);
    }
    private void UpdateGemstoneItem()
    {
        for (int i = 0; i < curGemstoneList.Count; i++)
        {
            PushGemstone(curGemstoneList[i]);
        }
        curGemstoneList.Clear();
        List<GuessingPuzzleNode> puzzleNodes = RandomUniqueValues(grids, gemstoneId.Count);
        for (int i = 0; i < puzzleNodes.Count; i++)
        {
            GameObject go;
            if (gemstoneList.Count > 0)
            {
                go = gemstoneList[0];
                gemstoneList.RemoveAt(0);
            }
            else
            {
                go = GameObject.Instantiate(gemstone_item);
            }
         
            go.transform.SetParent(puzzleNodes[i].transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.gameObject.SetActive(true);
            puzzleNodes[i].itemId = gemstoneId[i];
            Image image = go.GetComponent<Image>();
            itmeconfigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", gemstoneId[i]);
            image.sprite = UiManager.LoadSprite("item_icon", cfg.icon);
            curGemstoneList.Add(go);

        }
        mask.SetActive(true);
        time_pro.fillAmount = 1;
        mTick.gameObject.SetActive(true);
        TcikFun(0);
        timerInfo = UiManager.customTimer.RegisterTimer(TcikFun, 1, 3, () =>
        {
            mTick.gameObject.SetActive(false);
            mask.SetActive(false);
            for (int i = 0; i < curGemstoneList.Count; i++)
            {
                curGemstoneList[i].SetActive(false);
            }
            tween = time_pro.DOFillAmount(0.05f, 30);
            tween.SetEase(Ease.Linear);
            tween.onComplete = () =>
            {
                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(diff), () =>
                {
                    UpdataDiff(false);

                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });
            };

        });
       

    }
    private void TcikFun(int ti)
    {
        int fl = 3 - ti - 1;
        mTick.text = Common.Instance.SplitNumber(fl);

    }
    private void UpdataDiff(bool isAdd = true)
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;

        }
       
  
        if (isAdd == true)
        {
            diff++;

        }
        for (int i = 0; i < ditt_itme_list.Count; i++)
        {
            Image bg = ditt_itme_list[i].transform.Find("diff_bg").GetComponent<Image>();
            Transform tra = ditt_itme_list[i].transform.Find("diff_num");
            if (i + 1  > diff)
            {
          
                tra.gameObject.SetActive(false);
                bg.sprite = UiManager.LoadSprite("guessing_puzzle", "guessing_puzzle4");
            }
            else
            {
                tra.gameObject.SetActive(true);
                if (i + 1 == diff )
                {
                    bg.sprite = UiManager.LoadSprite("guessing_puzzle", "guessing_puzzle3");
                }
                else
                {
                    bg.sprite = UiManager.LoadSprite("guessing_puzzle", "guessing_puzzle2");
                }
            }
            bg.SetNativeSize();
        }
        gemstoneId.Clear();
        curRightNum = 0;
        if (diff == 1)
        {
            rowNum = 6;
            colNum = 4;
            grids = new GuessingPuzzleNode[rowNum, colNum];
            grid_root = grid_root1;
            gemstoneId.Add(15); //金属性宝石
            gemstoneId.Add(16); //土属性宝石
            gemstoneId.Add(17); //水属性宝石
            diff_bg.texture = UiManager.getTextureByNmae("guessing_puzzle_texture", "guessing_puzzle_diffbg1");
       
        }
        else if (diff == 2)
        {
            rowNum = 6;
            colNum = 4;
            grid_root = grid_root2;
            grids = new GuessingPuzzleNode[rowNum, colNum];
            gemstoneId.Add(15); //金属性宝石
            gemstoneId.Add(16); //土属性宝石
            gemstoneId.Add(17); //水属性宝石
            gemstoneId.Add(18); //木属性宝石
            diff_bg.texture = UiManager.getTextureByNmae("guessing_puzzle_texture", "guessing_puzzle_diffbg2");
        }
        else if (diff == 3)
        {
            rowNum = 8;
            colNum = 5;
            grid_root = grid_root3;
            grids = new GuessingPuzzleNode[rowNum, colNum];
            gemstoneId.Add(14); //火属性宝石
            gemstoneId.Add(15); //金属性宝石
            gemstoneId.Add(16); //土属性宝石
            gemstoneId.Add(17); //水属性宝石
            gemstoneId.Add(18); //木属性宝石
            diff_bg.texture = UiManager.getTextureByNmae("guessing_puzzle_texture", "guessing_puzzle_diffbg3");

        }
        else
        {

            return;
        }
        diff_bg.SetNativeSize();


      
        InitGrid();
   
        UpdateGemstoneItem();



    }
    private void UpdatGrid(GuessingPuzzleNode node)
    {
        GameObject go = node.gameObject;

       // BoxCollider boxCollider = go.GetComponent<BoxCollider>();
        //ridLayoutGroup gridLayout = grid_root.GetComponent<GridLayoutGroup>();
     //   boxCollider.size = gridLayout.cellSize;

    }
    public  List<GuessingPuzzleNode> RandomUniqueValues(GuessingPuzzleNode[,] array, int N)
    {
        List<GuessingPuzzleNode> uniqueValues = new List<GuessingPuzzleNode>();
        System.Random rand = new System.Random();

        int totalElements = array.GetLength(0) * array.GetLength(1);

        if (N > totalElements)
        {
            Console.WriteLine("要求的数量大于数组中的元素总数！");
            return uniqueValues;
        }

        while (uniqueValues.Count < N)
        {
            int randomIndex = rand.Next(totalElements);
            int row = randomIndex / array.GetLength(1);
            int col = randomIndex % array.GetLength(1);
          
            GuessingPuzzleNode value = array[row, col];
            if (!uniqueValues.Contains(value))
            {
                uniqueValues.Add(value);
            }
           
        }
        return uniqueValues;
    }
    private GuessingPuzzleNode PopGrid()
    {
        if (gridNodeList.Count > 0)
        {
            GuessingPuzzleNode node1 = gridNodeList[0];
            node1.gameObject.SetActive(true);
            node1.gameObject.transform.SetParent(grid_root);
            gridNodeList.RemoveAt(0);
            return node1;
        }
        GameObject go = GameObject.Instantiate(grid);
        go.SetActive(true);
        go.transform.SetParent(grid_root);
        go.transform.localScale = Vector3.one;
        GuessingPuzzleNode node = go.GetComponent<GuessingPuzzleNode>();
        MouseEnterDetector hover = go.GetComponent<MouseEnterDetector>();
        hover.onHover.AddListener(HoverHandler);
        return node;

    }

    private void HoverHandler(string state, MouseEnterDetector hover)
    {
        GameObject go = hover.gameObject;
        GuessingPuzzleNode curGrid = go.GetComponent<GuessingPuzzleNode>();
    
        if (state == "Enter")
        {
            if (curDragObj != null)
            {
                enterGrid = curGrid;
                Debug.Log("Mouse entered!");
            }
         
        }
        else if (state == "Exit")
        {
            if (curDragObj != null)
            {
                if (enterGrid == curGrid)
                {
                    enterGrid = null;
                }
                Debug.Log("Mouse exited!");
            }
        
   
            //   childTransform.gameObject.SetActive(false);

        }

    }
    private void PushGrid(GuessingPuzzleNode node)
    {
        gridNodeList.Add(node);
        node.gameObject.SetActive(false);

    }
    private GameObject PopGemstone()
    {
        if (gemstoneList.Count > 0)
        {
            GameObject go1 = gemstoneList[0];
            gridNodeList.RemoveAt(0);
            return go1;
        }
        GameObject go = GameObject.Instantiate(gemstone_item);
        return go;

    }
    private void PushGemstone(GameObject node)
    {
        gemstoneList.Add(node);
        node.gameObject.SetActive(false);

    }
    private void InitUI() 
    {
        consumeMap.Clear();
        consumeMap.Add(14,0); //火属性宝石
        consumeMap.Add(15,0); //金属性宝石
        consumeMap.Add(16,0); //土属性宝石
        consumeMap.Add(17,0); //水属性宝石
        consumeMap.Add(18,0); //木属性宝石
        float ratio = (float)Screen.height / 1080;
        
        for (int i = 0; i < bag_gemstone_list.Count; i++)
        {
            bag_gemstone_list[i].initialPos = bag_gemstone_list[i].transform.position;
            bag_gemstone_list[i].onDragEvent.AddListener(DragHandler);
            if (Screen.height < 1080)
            {
                bag_gemstone_list[i].baseScale = 1/ratio;

            }

        }
        UpdateGemstoneNum();
    }
    private void UpdateGemstoneNum()
    {
        for (int i = 0; i < bag_gemstone_list.Count; i++)
        {
            ShowBagGemstoneNum(bag_gemstone_list[i],i);
        }
    }
    private void ShowBagGemstoneNum(UIDragHandler bag_gemstone,int index)
    {

        TextMeshProUGUI te = gemstoneNumList[index];
        te.text = GetItmeNum(bag_gemstone.itemId).ToString();
       
       

    }
    private int GetItmeNum(int itemId)
    {
        ItemNode item = BagManage.Instance.GetItemById(itemId);
        int num = 0;
       if (item != null)
        {
            num = item.count - consumeMap[itemId];
       }
        return num;
    }
    private void DragHandler(string state, UIDragHandler uiDragHandler)
    {
        GameObject go = uiDragHandler.gameObject;
        Image image = go.transform.GetComponent<Image>();
        if (state == "PointerDown")
        {
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
            if (enterGrid != null)
            {
                if (GetItmeNum(uiDragHandler.itemId) < 1)
                {
                    Common.Instance.ShowTips("材料不足");
                    image.raycastTarget = true;
                    curDragObj.transform.position = uiDragHandler.initialPos;
                    curDragObj = null;
                    return;
                }
                if (!consumeMap.ContainsKey(uiDragHandler.itemId))
                {
                    consumeMap.Add(uiDragHandler.itemId, 1);
                }
                else
                {
                    consumeMap[uiDragHandler.itemId]++;
                }
      
                if (enterGrid.itemId > 0 &&enterGrid.itemId == uiDragHandler.itemId)
                {
                    Transform childTransform = enterGrid.transform.GetChild(0);
                    childTransform.gameObject.SetActive(true);
                    curRightNum++;
                    if (curRightNum == gemstoneId.Count)
                    {
                        if (diff == 3)
                        {
                            Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(diff), () =>
                            {
                                UpdataDiff(false);

                            }, () => { CloseSelf(); }, () =>
                            {
                                UpdataDiff();

                            });
                        }
                        else
                        {
                            Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(diff), () =>
                            {
                                UpdataDiff(false);

                            }, () => { CloseSelf(); }, () =>
                            {
                                UpdataDiff();

                            });
                        }
                        

                    }
                   
                    
                    Debug.Log("成功 itemId:" + uiDragHandler.itemId);
                }
                else
                {
                   
                }
                UpdateGemstoneNum();

            }
            enterGrid = null;
            image.raycastTarget = true;
            curDragObj.transform.position = uiDragHandler.initialPos;
            curDragObj = null;
         
            Debug.Log("Pointer Up");
        }
    }
    //{
    //    for (int i = 0; i < hovers.Count; i++)
    //    {
    //        hovers[i].onHover.AddListener(HoverHandler);
    //    }
    //    for (int i = 0; i < uIDrags.Count; i++)
    //    {
    //        uIDrags[i].initialPos = uIDrags[i].transform.position;
    //        uIDrags[i].onDragEvent.AddListener(DragHandler);
    //    }



    // }
    public override void OnDestroyImp()
    {
        timerInfo.isStop = true;
    }
}
