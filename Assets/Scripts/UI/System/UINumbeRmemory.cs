using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NumbeRmemoryLevel
{
    public int[,] level;
    public bool isSheng;
    public NumbeRmemoryLevel(int rows, int columns)
    {
        level = FillArrays(rows, columns);

    }

    public int[,] FillArrays(int rows, int columns)
    {
        int[,] result = new int[rows, columns];
        System.Random random = new System.Random();
        HashSet<int> usedNumbers = new HashSet<int>(); // 用于存储已使用的随机数
        int num = random.Next(1, 3); // 生成的随机数在1和2之间（包括1和2）
        if (num == 1)
        {
            isSheng = false;
        }
        else
        {
            isSheng = true;

        }
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int randomNumber;
                do
                {
                    randomNumber = random.Next(101); // 生成0到100之间的随机数
                } while (usedNumbers.Contains(randomNumber)); // 检查随机数是否已经存在

                result[i, j] = randomNumber;
                usedNumbers.Add(randomNumber);
            }
        }

        return result;
    }


}
public class UINumbeRmemory : UIBase
{
    private List<NumbeRmemoryItemNode> currGrid = new List<NumbeRmemoryItemNode>();
    private List<DragonBonesController> currDragon = new List<DragonBonesController>();
    private List<DragonBonesController> popBonesPool = new List<DragonBonesController>();
    private List<NumbeRmemoryItemNode> wayGridPool = new List<NumbeRmemoryItemNode>();
    NumbeRmemoryLevel level;
    public List<RectTransform> grid_root;
    public List<GameObject> bg_list;
    private int diff = 0;
    public GameObject grid_item;
    public List<Button> close_btns;
    private int currValue = 0;
    List<int> numbers = new List<int>();
    public Image diffImage;
    public Image shengjingImage;
    public Image pro;
    private float time = 10;
    private Tween proTween;
    public Button close_btn;
    public Button rule_btn;
    public RectTransform new_bie;
    private bool isNewBie = false;
    private int newBieIndex = 1;
    private Dictionary<int, GameObject> gridMap = new Dictionary<int, GameObject>();
    public override void OnStart()
    {

        for (int i = 0; i < close_btns.Count; i++)
        {
            close_btns[i].onClick.AddListener(CloseSelf);
        }

        MissionManage.ShowDescription(()=>{

            UpdataDiff();
        });
        rule_btn.onClick.AddListener(() =>
        {
            proTween.Pause();
            MissionManage.ShowDescription(() =>
            {
                if (proTween != null)
                {
                    proTween.Play();
                }
            });

        });
       

    }

    public void SetIsNewBie(bool isNewBie)
    {
        this.isNewBie = isNewBie;


    }
    private void UpdataDiff(bool isAdd = true)
    {
        proTween.Kill();
        proTween = null;
        
        if (isAdd)
        {
            diff++;
        }
       

        if (diff == 1)
        {
            time = 20;
            level = new NumbeRmemoryLevel(3, 3);
            diffImage.sprite = UiManager.LoadSprite("numbe_rmemory", "NumbeRmemory_13");
        }
        else if (diff == 2)
        {
            time = 35;
            level = new NumbeRmemoryLevel(4, 4);
            diffImage.sprite = UiManager.LoadSprite("numbe_rmemory", "NumbeRmemory_14");
        }
        else if (diff == 3)
        {
            time = 10;
            level = new NumbeRmemoryLevel(5, 5);
            diffImage.sprite = UiManager.LoadSprite("numbe_rmemory", "NumbeRmemory_15");
        }
        numbers.Clear();
        InitGrid();
    }
    private void InitGrid()
    {
        pro.fillAmount = 1;
        proTween = pro.DOFillAmount(0, time).SetEase(Ease.Linear);
        proTween.onComplete = ()=>
        {
            Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(diff), () =>
            {
                UpdataDiff(false);

            }, () => { CloseSelf(); }, () =>
            {
                UpdataDiff();

            });
        };
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
        for (int i = 0; i < bg_list.Count; i++)
        {
            bg_list[i].gameObject.SetActive(diff - 1 == i);
        }
        gridMap.Clear();
        for (int i = 0; i < level.level.GetLength(0); i++)
        {
            for (int z = 0; z < level.level.GetLength(1); z++)
            {
                NumbeRmemoryItemNode grid = PopGridItem();
                GameObject go = grid.gameObject;
                go.transform.SetParent(grid_root[diff - 1]);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                grid.number.text = level.level[i,z].ToString();

                grid.gridZ = z;
                grid.gridI = i;
                grid.value = level.level[i, z];
                numbers.Add(level.level[i, z]);
                gridMap.Add(level.level[i, z], go);

            }
        }
        if (level.isSheng)
        {
            shengjingImage.sprite = UiManager.LoadSprite("numbe_rmemory", "NumbeRmemory_8");
            
            numbers.Sort();
        }
        else
        {
            shengjingImage.sprite = UiManager.LoadSprite("numbe_rmemory", "NumbeRmemory_4");
            numbers.Sort((a, b) => b.CompareTo(a));
        }
  
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(grid_root[diff - 1]);
        if (isNewBie)
        {
            if (newBieIndex < 3)
            {
                new_bie.gameObject.SetActive(true);
                new_bie.position = new Vector3(gridMap[numbers[0]].transform.position.x + 0.5f, gridMap[numbers[0]].transform.position.y-0.7f,0); 
            }
            else
            {
                new_bie.gameObject.SetActive(false);
            }
        }
    }
    private void ClickGrid(NumbeRmemoryItemNode gridNode)
    {
        if (gridNode.isClick )
        {
            return;
        }
        DragonBonesController dragon = PopGemstoneItem();
        dragon.transform.SetParent(gridNode.transform);
        dragon.transform.localPosition = Vector3.zero;
        dragon.transform.localScale = Vector3.one;
        gridNode.pop.gameObject.SetActive(false);
        gridNode.isClick = true;
        if (numbers[0] == gridNode.value)
        {

            numbers.RemoveAt(0);
            newBieIndex++;
            if (isNewBie)
            {
                if (newBieIndex < 3)
                {
                    new_bie.position = new Vector3(gridMap[numbers[0]].transform.position.x + 0.5f, gridMap[numbers[0]].transform.position.y - 0.7f, 0);
                }
                else
                {
                    new_bie.gameObject.SetActive(false);
                }
            }
            dragon.PlayAnimation("01_Bomb", false, () =>
            {
                PushGemstoneItem(dragon);
                currDragon.Remove(dragon);
            });
            currValue = gridNode.value;
            if (numbers.Count == 0)
            {
                proTween.Kill();
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
        else
        {
            dragon.PlayAnimation("02_Wrong", false, () =>
            {
               
                gridNode.pop.gameObject.SetActive(true);
                PushGemstoneItem(dragon);
                currDragon.Remove(dragon);
                gridNode.isClick = false;
            });
        }

    }
    private NumbeRmemoryItemNode PopGridItem()
    {
        if (wayGridPool.Count > 0)
        {
            NumbeRmemoryItemNode node = wayGridPool[0];
            node.pop.gameObject.SetActive(true);
            node.gameObject.SetActive(true);
            wayGridPool.RemoveAt(0);
            currGrid.Add(node);
            return node;
        }
        GameObject go = GameObject.Instantiate(grid_item);
        NumbeRmemoryItemNode wayItemNode = go.GetComponent<NumbeRmemoryItemNode>();
        wayItemNode.clickEvent.AddListener(ClickGrid);
        currGrid.Add(wayItemNode);
        return wayItemNode;
    }
    private void PushGridItem(NumbeRmemoryItemNode node)
    {
        node.isClick = false;
        wayGridPool.Add(node);
        node.bones_root.gameObject.SetActive(true);
        //currGrid.Remove(node);
        node.gameObject.SetActive(false);
    }
    private DragonBonesController PopGemstoneItem()
    {
        if (popBonesPool.Count > 0)
        {
            DragonBonesController node = popBonesPool[0];
            node.gameObject.SetActive(true);
            popBonesPool.RemoveAt(0);
            currDragon.Add(node);
            return node;
        }
        DragonBonesController go = UiManager.LoadBonesByNmae("numbe_rmemory_pop");
        currDragon.Add(go);
        return go;
    }
    private void PushGemstoneItem(DragonBonesController node)
    {
        popBonesPool.Add(node);
        node.gameObject.SetActive(false);
    }
}
