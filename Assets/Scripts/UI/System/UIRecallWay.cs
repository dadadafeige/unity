using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecallWayLevel
{
    

    List<int[,]> levelList = new List<int[,]>();
    int[,] level = new int[6, 6]{
       {0,1,2,1,1,0 },
       {1,1,0,0,0,2 },
       {4,0,0,1,0,1 },
       {1,0,0,1,0,1 },
       {1,1,0,1,0,5 },
       {0,0,2,1,0,1 },
     
    };
    int[,] level2 = new int[6, 7]{
       {1,1,1,1,1,1,0},
       {0,0,0,0,0,0,2 },
       {0,1,1,1,0,1 ,1},
       {0,0,0,1,0,0 ,5},
       {1,1,0,2,1,1 ,0},
       {4,0,0,1,0,0 ,0},

    };
    int[,] level3 = new int[6, 8]{
       {1,1,0,0,0,1,0,5},
       {4,0,0,1,0,1,0,1},
       {1,1,1,0,0,1,0,0},
       {0,2,0,0,1,1,1,0},
       {1,1,0,1,1,0,0,0},
       {1,1,0,0,0,0,1,1},

    };
    public RecallWayLevel()
    {
        levelList.Add(level);


    }
    public int[,] GetLevelArr(int diff)
    {
        if (diff == 1)
        {
            return level;
        }
        else if (diff == 2)
        {
            return level2;

        }
        else
        {
            return level3;

        }
       
    }


}
public class UIRecallWay : UIBase
{
   
    public GameObject grid_item;
    public List<RectTransform> grid_root = new List<RectTransform>();
    private int row = 6;
    private int col = 6;
    RecallWayLevel level = new RecallWayLevel();
    private RecallWayItemNode[,] grid2D = new RecallWayItemNode[6, 6];
    private List<DragonBonesController> pugongyingPool = new List<DragonBonesController>();
    private List<DragonBonesController> currDragon = new List<DragonBonesController>();
    private List<RecallWayItemNode> currGrid = new List<RecallWayItemNode>();
    private List<RecallWayItemNode> wayGridPool = new List<RecallWayItemNode>();
    private bool isOpen = false;
    DelayedCoroutineData delayed;
    private bool isCanClick = true;
    public Image pro;
    public List<GameObject> hp_itme_list = new List<GameObject>();
    public List<GameObject> bg_root = new List<GameObject>();
    private int curHp = 3;
    private int diff = 0;
    private int curI;
    private int curZ;
    Tween tween;
    public List<Button> close_btns;
    public List<GameObject> can_list = new List<GameObject>();
    public Button rule_btn;
    // Start is called before the first frame update
    public override void OnStart()
    {
        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                UpdataDiff();
            });
        });

        rule_btn.onClick.AddListener(() =>
        {
            tween.Pause();
            MissionManage.ShowDescription(() =>
            {
                tween.Play();

            });
        });


        for (int i = 0; i < close_btns.Count; i++)
        {
            close_btns[i].onClick.AddListener(CloseSelf);
        }
   
      //  InitGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdataDiff(bool isAdd = true)
    {
        if (isAdd)
        {
            diff++;

        }
       
    
        if (diff == 1)
        {
           
        }
        else if (diff == 2)
        {
            //if (isAdd)
            //{
            //    Common.Instance.ShowTips("难度提升");
            //}
        }
        else if (diff == 3)
        {
            //if (isAdd)
            //{
            //    Common.Instance.ShowTips("难度提升");
            //}
        }
        else
        {
            

            return;
        }
        for (int i = 0; i < bg_root.Count; i++)
        {
            bg_root[i].SetActive(diff - 1 == i);
        }
        curHp = 3;
        UpdataHp();
        InitGrid();
        if (tween != null)
        {
            tween.Kill();
        }
        pro.fillAmount = 0.92f;
        isCanClick = false;
        GameObject ling_go = bg_root[diff - 1].transform.Find("line").gameObject;
        ling_go.SetActive(true);
        delayed = DelayedActionProvider.Instance.DelayedAction(() =>
        {
          
            ling_go.SetActive(false);
            ShowCanClickGrid(curI, curZ);
            tween = pro.DOFillAmount(0.33f, 20);
            tween.onComplete = () =>
            {
                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(diff), () =>
                {
                    UpdataDiff(false);

                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });
                tween = null;

            };
            isCanClick = true;
            delayed = null;
            for (int i = 0; i < currGrid.Count; i++)
            {
                if (currGrid[i].gridType != 4)
                {
                    currGrid[i].yun.gameObject.SetActive(true);
                    currGrid[i].bones_root.gameObject.SetActive(false);
                }
              
            }
        },3);
        


    }
    private void UpdataHp()
    {
        if (curHp == 0)
        {
            if (tween != null)
            {
                tween.Kill();
            }

            Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(diff), () =>
            {
                UpdataDiff(false);

            }, () => { CloseSelf(); }, () =>
            {
                UpdataDiff();

            });
        }
        for (int i = 0; i < hp_itme_list.Count; i++)
        {
            GameObject hp_icon = hp_itme_list[i].transform.Find("hp_icon").gameObject;
            hp_icon.SetActive(i + 1 <= curHp);
        }

    }
    private void InitGrid()
    {
        int[,] arr = level.GetLevelArr(diff);
        System.Random random = new System.Random();
        for (int i = 0; i < currGrid.Count; i++)
        {
            PushGridItem(currGrid[i]);
        }
        for (int i = 0; i < currDragon.Count; i++)
        {
            PushGemstoneItem(currDragon[i]);
        }
        currDragon.Clear();
        currGrid.Clear();
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int z = 0; z < arr.GetLength(1); z++)
            {
                RecallWayItemNode grid = PopGridItem();
                GameObject go = grid.gameObject;
                go.transform.SetParent(grid_root[diff-1]);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                TextMeshProUGUI text = go.transform.Find("index").GetComponent<TextMeshProUGUI>();
                Image yun = grid.yun;
                int randomNumber = random.Next(1, 4); // 生成1到3的随机数
                yun.sprite = UiManager.LoadSprite("recall_way", "recall_way_cloud" + randomNumber);
                yun.SetNativeSize();
                yun.gameObject.SetActive(false);
                grid.clickEvent.AddListener(ClickGrid);
                grid.gridType = arr[i, z];
                if (arr[i,z] > 0)
                {

                    if (arr[i, z] == 1) 
                    {
                        DragonBonesController dragon = PopGemstoneItem();
                        dragon.gameObject.SetActive(true);
                        dragon.transform.SetParent(grid.bones_root);
                        dragon.transform.localScale = Vector3.one;
                        dragon.transform.localPosition = new Vector3(0, 0, 0);
                        dragon.PlayAnimation("01_PGY_Idle",true);
                        grid.dragon = dragon;
                    }
                    else if (arr[i, z] == 2)
                    {
                        DragonBonesController dragon = PopGemstoneItem();
                        dragon.gameObject.SetActive(true);
                        dragon.transform.SetParent(grid.bones_root);
                        dragon.transform.localScale = Vector3.one;
                        dragon.transform.localPosition = new Vector3(0, 0, 0);
                        dragon.PlayAnimation("02_DaoJu_Idle", true);
                        grid.dragon = dragon;
                    }
                    else if (arr[i, z] == 4)
                    {

                        curI = i;
                        curZ = z;
                 
                        grid.isClick = true;

                    }
                 
                }
                text.text = z + "," + i;
               // grid2D[z, i] = grid;
                grid.gridZ = z;
                grid.gridI = i;
         
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(grid_root[diff -1]);
    }
    private RecallWayItemNode PopGridItem()
    {
        if (wayGridPool.Count > 0)
        {
            RecallWayItemNode node = wayGridPool[0];
            node.gameObject.SetActive(true);
            wayGridPool.RemoveAt(0);
            currGrid.Add(node);
            return node;
        }
        GameObject go = GameObject.Instantiate(grid_item);
        RecallWayItemNode wayItemNode = go.GetComponent<RecallWayItemNode>();
        currGrid.Add(wayItemNode);
        return wayItemNode;
    }
    private void PushGridItem(RecallWayItemNode node)
    {
        node.isClick = false;
        wayGridPool.Add(node);
        node.bones_root.gameObject.SetActive(true);
        //currGrid.Remove(node);
        node.gameObject.SetActive(false);
    }
    private DragonBonesController PopGemstoneItem()
    {
        if (pugongyingPool.Count > 0)
        {
            DragonBonesController node = pugongyingPool[0];
            node.gameObject.SetActive(true);
            pugongyingPool.RemoveAt(0);
            currDragon.Add(node);
            return node;
        }
        DragonBonesController go = UiManager.LoadBonesByNmae("recall_way_grid_bones");
        currDragon.Add(go);
        return go;
    }
    private void PushGemstoneItem(DragonBonesController node)
    {
        pugongyingPool.Add(node);
        node.gameObject.SetActive(false);
    }
    private void ShowCanClickGrid(int i, int z)
    {
        for (int c = 0; c < can_list.Count; c++)
        {
            can_list[c].SetActive(false);
        }
        can_list.Clear();
        RecallWayItemNode upNode = GetGridNodeByZanI(i, z + 1);
        RecallWayItemNode downNode = GetGridNodeByZanI(i, z - 1);
        RecallWayItemNode leftNode = GetGridNodeByZanI(i + 1, z);
        RecallWayItemNode rightNode = GetGridNodeByZanI(i - 1, z);
        if (upNode != null)
        {
            upNode.can_click.SetActive(true);
            can_list.Add(upNode.can_click);

        }
        if (downNode != null)
        {
            downNode.can_click.SetActive(true);
            can_list.Add(downNode.can_click);

        }
        if (leftNode != null)
        {
            leftNode.can_click.SetActive(true);
            can_list.Add(leftNode.can_click);

        }
        if (rightNode != null)
        {
            rightNode.can_click.SetActive(true);
            can_list.Add(rightNode.can_click);

        }

    }
    private RecallWayItemNode GetGridNodeByZanI(int nodeI, int nodeZ)
    {

        for (int i = 0; i < currGrid.Count; i++)
        {
            if (currGrid[i].gridZ == nodeZ && currGrid[i].gridI == nodeI)
            {
                return currGrid[i];
            }
        }

        return null;

    }
    public bool IsAdjacent(int i, int z)
    {
        //RecallWayItemNode upNode = GetGridNodeByZanI(i, z + 1);
        //RecallWayItemNode downNode = GetGridNodeByZanI(i, z - 1);
        //RecallWayItemNode leftNode = GetGridNodeByZanI(i + 1, z);
        //RecallWayItemNode rightNode = GetGridNodeByZanI(i - 1, z);
        //if ((upNode != null && upNode.isClick == true) || (downNode != null && downNode.isClick == true)
        //    || (leftNode != null && leftNode.isClick == true) || (rightNode != null && rightNode.isClick == true))
        //{
        //    return true;
        //}
        //return false;




        // 检查是否在上下左右四个方向
        if ((i == curI && (z == curZ + 1 || z == curZ - 1)) || // 上下
            (z == curZ && (i == curI + 1 || i == curI - 1)))   // 左右
        {
            return true;
        }
        return false;
    }
    public override void OutUI()
    {
        for (int c = 0; c < can_list.Count; c++)
        {
            can_list[c].SetActive(false);
        }
    }
    public override void GoInUI()
    {

        for (int c = 0; c < can_list.Count; c++)
        {
            can_list[c].SetActive(false);
        }
    }
    private void ClickGrid(RecallWayItemNode gridNode)
    {
        if (!IsAdjacent(gridNode.gridI,gridNode.gridZ))
        {
            return;
        }
        if (!isCanClick)
        {
            return;
        }
        if (gridNode.isClick && gridNode.gridType != 1)
        {
            curI = gridNode.gridI;
            curZ = gridNode.gridZ;
            ShowCanClickGrid(curI, curZ);
            return;
        }
        Transform yun = gridNode.yun.transform;
        yun.gameObject.SetActive(false);
        if (gridNode.gridType != 1)
        {
            
            curI = gridNode.gridI;
            curZ = gridNode.gridZ;
            ShowCanClickGrid(curI, curZ);

        }
 
        gridNode.isClick = true;
        if (gridNode.gridType>0)
        {
     
            gridNode.bones_root.gameObject.SetActive(true);
            if (gridNode.gridType == 1)
            {
                curHp--;
                UpdataHp();
                DragonBonesController bones;
                if (gridNode.ball_bomb_bones!= null)
                {
                    bones = gridNode.ball_bomb_bones;
                    bones.gameObject.SetActive(true);
                }
                else
                {
                    bones = UiManager.LoadBonesByNmae("ball_bomb_bones");
                    bones.transform.SetParent(gridNode.transform);
                    bones.transform.localPosition = Vector3.zero;
                    bones.transform.localScale = Vector3.one;
                    gridNode.ball_bomb_bones = bones;
                }
                bones.PlayAnimation("01_Play", false, () =>
                {
                    bones.gameObject.SetActive(false);


                });

            }
            else if (gridNode.gridType == 2)
            {
                isCanClick = false;
                tween.Pause();
                for (int i = 0; i < currGrid.Count; i++)
                {
                    RecallWayItemNode grid = currGrid[i];
                    if (!grid.isClick)
                    {
                        grid.yun.gameObject.SetActive(false);
                        if (grid.dragon != null)
                        {

                            grid.bones_root.gameObject.SetActive(true);
                        }
                    }
                    
                }
                delayed = DelayedActionProvider.Instance.DelayedAction(() =>
                {
                    tween.Play();
                    delayed = null;
                    isCanClick = true;
                    for (int i = 0; i < currGrid.Count; i++)
                    {
                        RecallWayItemNode grid = currGrid[i];
                        if (!grid.isClick)
                        {
                            grid.yun.gameObject.SetActive(true);
                            if (grid.dragon != null)
                            {
                                grid.bones_root.gameObject.SetActive(false);
                            }
                        }
                        
                    }
                }, 3);
            }
            else if (gridNode.gridType == 5)
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
        }

    }
    public override void OnDestroyImp()
    {
        if (delayed != null)
        {
            delayed.isStop = true;

        }
    }
}
