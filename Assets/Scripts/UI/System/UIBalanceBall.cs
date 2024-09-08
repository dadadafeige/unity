using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIBalanceBall : UIBase
{
    public GameObject grid_item;
    public RectTransform grid_root;
    private CollectMaterialGridNode[,] grid2D = new CollectMaterialGridNode[8, 6];
    public RectTransform barricade_grid;
    public GameObject barricade;
    private List<CollectMaterialEnemyNode> enemyPool = new List<CollectMaterialEnemyNode>();
    private List<CollectMaterialEnemyNode> currEnemyList = new List<CollectMaterialEnemyNode>();
    public GameObject enemy_image;
    public RectTransform enemy_root;
    private int enemyNum;
    public List<RectTransform> item_root;
    public int[] shizhuNum = new int[3] { 3, 0, 0 }; //key == 0 猪头 key==1 麒麟 key == 2 盘龙
    private List<DragonBonesController> shizhuPool = new List<DragonBonesController>();
    public Image pro;
    public TextMeshProUGUI ballNum;
    public Image diff;
    private int curr_diff = 0;
    private bool isOver = false;
    private List<DelayedCoroutineData> delayeds = new List<DelayedCoroutineData>();
    private Tween proTween;
    private int[] buyTypItem = new int[3] { 3,0,0};
    public Button close_btn;
    public Button rule_btn;

    // Start is called before the first frame update
    public override void OnStart()
    {
       
        InitGrid();
        InitBarricadeGrid();
        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                UpdataDiff();
                InitItem();
            });
        });
    //    UpdataDiff();
        close_btn.onClick.AddListener(CloseSelf);
        rule_btn.onClick.AddListener(() =>
        {
            for (int i = 0; i < currEnemy.Count; i++)
            {
                if (currEnemy[i].tween != null)
                {
                    currEnemy[i].tween.Pause();
                }

            }
          
            if (proTween != null)
            {
                proTween.Pause();

            }

          
            MissionManage.ShowDescription(() =>
            {
                for (int i = 0; i < currEnemy.Count; i++)
                {
                    if (currEnemy[i].tween != null)
                    {
                        currEnemy[i].tween.Play();
                    }

                }
                if (proTween != null)
                {
                    proTween.Play();

                }

            });



        });
        for (int i = 0; i < item_root.Count; i++)
        {
            Button btn = item_root[i].GetComponent<Button>();
            DragonBonesController bones = null;
            if (i != 0)
            {
                bones = item_root[i].Find("ball_lock_bones").GetComponent<DragonBonesController>();

            }
            int index = i;
        
            btn.onClick.AddListener(() =>
            {
             
                if (index == 1)
                {
                    if (buyTypItem[index] > 0)
                    {

                    }
                    else
                    {
                        if (GameManage.userData.gold >= 100)
                        {
                            UITipsBoard tipsBoard = UiManager.OpenUI<UITipsBoard>("UITipsBoard");
                            tipsBoard.SetData("是否购买麒麟柱", 100, () => {
                            shizhuNum[index]++;
                            buyTypItem[index]++;
                            bones.PlayAnimation("02_Open", false);
                            InitItem();
                            GameManage.userData.SetAddGoldValue(-100);

                        });
                        }
                        else
                        {
                            Common.Instance.ShowTips("金币不足100");
                        };

                    }

                }
                else if (index == 2)
                {

                    if (buyTypItem[index] > 0)
                    {

                    }
                    else
                    {
                        if (GameManage.userData.gold >= 200)
                        {

                            UITipsBoard tipsBoard = UiManager.OpenUI<UITipsBoard>("UITipsBoard");
                            tipsBoard.SetData("是否购买蟠龙柱", 200, () => {
                                bones.PlayAnimation("02_Open", false);
                                shizhuNum[index]++;
                                buyTypItem[index]++;
                                InitItem();
                                GameManage.userData.SetAddGoldValue(-200);
                            });
                        }
                        else
                        {
                            Common.Instance.ShowTips("金币不足200");
                        }
                    }
                    
                };
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private int time = 0;
    private void UpdataDiff(bool isAdd = true)
    {
        if (isAdd)
        {
            curr_diff++;

        }
        for (int i = 0; i < delayeds.Count; i++)
        {
            delayeds[i].isStop = true;
        }
        for (int i = 0; i < currDragon.Count; i++)
        {
            PushGemstoneItem(currDragon[i]);
        }
        currDragon.Clear();
        for (int i = 0; i < shizhuNum.Length; i++)
        {
            shizhuNum[i] = buyTypItem[i];
        }
        isOver = false;
        InitItem();
        currDragon.Clear();
        delayeds.Clear();
        pro.fillAmount = 1;
        if (proTween != null)
        {
            proTween.Kill();
        }
        
        if (curr_diff == 1)
        {
            enemyNum = 3;
            time = 20;
            diff.sprite = UiManager.LoadSprite("common", "diff_1");

        }
        else if (curr_diff == 2)
        {
            diff.sprite = UiManager.LoadSprite("common", "diff_3");
            enemyNum = 4;
            time = 30;
        }
        else if (curr_diff == 3)
        {
            diff.sprite = UiManager.LoadSprite("common", "diff_5");
            enemyNum = 5;
            time = 40;
        }
        proTween = pro.DOFillAmount(0, time).SetEase(Ease.Linear);
        for (int i = 0; i < currEnemy.Count; i++)
        {
            if (currEnemy[i].tween != null)
            {
                currEnemy[i].tween.Kill();
            }

        }
        proTween.onComplete = () =>
        {
            proTween = null;
            if (curr_diff < 3)
            {
                for (int i = 0; i < currEnemy.Count; i++)
                {
                    if (currEnemy[i].tween != null)
                    {
                        currEnemy[i].tween.Kill();
                    }

                }
                proTween.Kill();
                Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(curr_diff), () =>
                {
                    UpdataDiff(false);

                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });
           
            }
            else
            {
                for (int i = 0; i < currEnemy.Count; i++)
                {
                    if (currEnemy[i].tween != null)
                    {
                        currEnemy[i].tween.Kill();
                    }

                }
                proTween.Kill();
                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(curr_diff), () =>
                {
                    UpdataDiff(false);

                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });
            }

        };
        ballNum.text = enemyNum.ToString();
        UpdatEnemyItem();
    }
    private void UpdatEnemyItem()
    {
        for (int i = 0; i < currEnemyList.Count; i++)
        {
            PushEnemyItem(currEnemyList[i]);
        }
        currEnemyList.Clear();
        List<CollectMaterialGridNode> nodes = RandomUniqueValues(grid2D, enemyNum);
        for (int i = 0; i < nodes.Count; i++)
        {
            CollectMaterialEnemyNode node = PopEnemyItem();
            DelayedActionProvider.Instance.DelayedAction(() => { node.speed = node.speed - (node.speed * 0.2f); },time/2);
            node.gameObject.transform.SetParent(enemy_root);
            node.transform.localScale = Vector3.one;
            node.gameObject.SetActive(true);
          //  node.UpdateEnemy();
            node.transform.localPosition = nodes[i].transform.localPosition;
            node.gridNode = nodes[i];
            currEnemyList.Add(node);
            EnemyTween(node);
        }
    }
    private void InitItem()
    {

        for (int i = 0; i < item_root.Count; i++)
        {
            RectTransform item = item_root[i];
            TextMeshProUGUI num = item.Find("num_bg/num").GetComponent<TextMeshProUGUI>();
            num.text = shizhuNum[i].ToString();
        }

    }
    private int GetShizhuType()
    {
        for (int i = 0; i < shizhuNum.Length; i++)
        {
            if (shizhuNum[i] > 0)
            {
                shizhuNum[i]--;
                InitItem();
                return i;
            }
        }
        return -1;
    }
    private void EnemyTween(CollectMaterialEnemyNode node)
    {
        // 当前点的坐标
        int currentX = node.gridNode.gridZ;
        int currentY = node.gridNode.gridI;

        // 随机选择一个方向（0：上，1：下，2：左，3：右）
        System.Random rand = new System.Random();
        int direction = rand.Next(0, 4);

        // 计算下一个点的坐标
        int nextX = currentX;
        int nextY = currentY;
        DragonBonesController dragon = node.transform.Find("ball_bones").GetComponent<DragonBonesController>();
        string animName = "02_TurnUp";

        switch (direction)
        {
           
            case 0: // 上
                animName = "02_TurnUp";
                nextY--;
                break;
            case 1: // 下
                animName = "01_TurnDown";
                nextY++;
                break;
            case 2: // 左
                animName = "04_TurnLeft";
                nextX--;
                break;
            case 3: // 右
                animName = "03_TurnRight";
                nextX++;
                break;
        }
        // 检查下一个点的坐标是否在范围内，并且每次只能移动1步
        if ((nextX == currentX - 1 || nextX == currentX + 1 || nextX == currentX) &&
            (nextY == currentY - 1 || nextY == currentY + 1 || nextY == currentY) &&
            nextX >= 0 && nextX < grid2D.GetLength(0) && nextY >= 0 && nextY < grid2D.GetLength(1))
        {

            dragon.PlayAnimation(animName, true);
            node.tween = node.transform.DOMove(grid2D[nextX, nextY].transform.position, node.speed);
            node.tween.SetEase(Ease.Linear);
            node.tween.onComplete = () =>
            {
                node.gridNode = grid2D[nextX, nextY];
                EnemyTween(node);

            };
        }
        else
        {
            // 不在范围内，重新选择一个新的方向并计算新的下一个点
            do
            {
                direction = rand.Next(0, 4);

                nextX = currentX;
                nextY = currentY;
                switch (direction)
                {
                    case 0: // 上
                        animName = "02_TurnUp";
                        nextY--;
                        break;
                    case 1: // 下
                        animName = "01_TurnDown";
                        nextY++;
                        break;
                    case 2: // 左
                        animName = "04_TurnLeft";
                        nextX--;
                        break;
                    case 3: // 右
                        animName = "03_TurnRight";
                        nextX++;
                        break;
                }
            } while (!((nextX == currentX - 1 || nextX == currentX + 1 || nextX == currentX) &&
                       (nextY == currentY - 1 || nextY == currentY + 1 || nextY == currentY) &&
                       nextX >= 0 && nextX < grid2D.GetLength(0) && nextY >= 0 && nextY < grid2D.GetLength(1)));


            dragon.PlayAnimation(animName, true);
            // 返回重新计算后的下一个点的索引
            Console.WriteLine("重新计算后的下一个点的坐标：(" + nextX + ", " + nextY + ")");
            node.tween = node.transform.DOMove(grid2D[nextX, nextY].transform.position, 1.5f);
            node.tween.SetEase(Ease.Linear);
            node.tween.onComplete = () =>
            {
                node.gridNode = grid2D[nextX, nextY];
                EnemyTween(node);

            };
        }
        node.direction = direction;
    }

    public static List<CollectMaterialGridNode> RandomUniqueValues(CollectMaterialGridNode[,] array, int N)
    {
        List<CollectMaterialGridNode> uniqueValues = new List<CollectMaterialGridNode>();
        System.Random rand = new System.Random();

        int totalElements = array.GetLength(0) * array.GetLength(1);

        if (N > totalElements)
        {
            Console.WriteLine("要求的数量大于数组中的元素总数！");
            return uniqueValues;
        }
        int backRow = -1;
        int backCol = -1;
        while (uniqueValues.Count < N)
        {
            int randomIndex = rand.Next(totalElements);
            int row = randomIndex / array.GetLength(1);
            int col = randomIndex % array.GetLength(1);
     
            if (row > 1 && col > 1)
            {
                CollectMaterialGridNode value = array[row, col];
                if (!uniqueValues.Contains(value))
                {

                    if (backRow == -1 || (Math.Abs(backRow - row ) >= 2 && Math.Abs(backCol - col) >= 2))
                    {
                        uniqueValues.Add(value);
                        backRow = row;
                        backCol = col;
                    }
                }
            }
        }

        return uniqueValues;
    }
    private void InitGrid()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int z = 0; z < 8; z++)
            {
                GameObject go = GameObject.Instantiate(grid_item);
                go.transform.SetParent(grid_root);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                CollectMaterialGridNode grid = go.GetComponent<CollectMaterialGridNode>();
                TextMeshProUGUI text = go.transform.Find("index").GetComponent<TextMeshProUGUI>();
                grid.clickEvent.AddListener(ClickGrid);
                text.text = z + "," + i;
                grid2D[z, i] = grid;
                grid.gridZ = z;
                grid.gridI = i;
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(grid_root);
    }
    private void ClickGrid(CollectMaterialGridNode gridNode)
    {
        int shizhuType = GetShizhuType();
        if (shizhuType == -1 )
        {
            return;
        }
        DragonBonesController dragon = PopGemstoneItem();
        dragon.transform.SetParent(gridNode.transform);
        dragon.transform.localScale = Vector3.one;
        dragon.transform.localPosition = new Vector3(0,-25,0);
        string animName = "01_ShiZhu";
        if (shizhuType == 0) //猪头
        {
            animName = "01_ShiZhu";
        }
        else if (shizhuType == 1)//麒麟
        {
            animName = "02_QLZhu";
        }
        else if (shizhuType == 2)//盘龙
        {
            animName = "03_PLZhu";
        }
        dragon.PlayAnimation(animName, false, () =>{
            DelayedCoroutineData delayed = DelayedActionProvider.Instance.DelayedAction(() =>
            {
                shizhuNum[shizhuType]++;
                InitItem();
                PushGemstoneItem(dragon);
                currDragon.Remove(dragon);
                delayeds.RemoveAt(0);

            }, 5);
            delayeds.Add(delayed);

        });


    }
    private List<DragonBonesController> currDragon = new List<DragonBonesController>();
    private DragonBonesController PopGemstoneItem()
    {
        if (shizhuPool.Count > 0)
        {
            DragonBonesController node = shizhuPool[0];
            node.gameObject.SetActive(true);
            shizhuPool.RemoveAt(0);
            currDragon.Add(node);
            return node;
        }
        DragonBonesController go = UiManager.LoadBonesByNmae("ball_shizhu_bones");
        currDragon.Add(go);
        return go;
    }
    private void PushGemstoneItem(DragonBonesController node)
    {
        shizhuPool.Add(node);
      
        node.gameObject.SetActive(false);
    }

    private void InitBarricadeGrid()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int z = 0; z < 5; z++)
            {
                GameObject go = GameObject.Instantiate(barricade);
                go.transform.SetParent(barricade_grid);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                Image image = go.GetComponent<Image>();
                int randomNumber = UnityEngine.Random.Range(1, 7);
                image.sprite = UiManager.LoadSprite("collect_material", "collect_material_barricade" + randomNumber);

            }
        }
    }
    public List<CollectMaterialEnemyNode> currEnemy = new List<CollectMaterialEnemyNode>();
    private CollectMaterialEnemyNode PopEnemyItem()
    {
        if (enemyPool.Count > 0)
        {
            CollectMaterialEnemyNode node = enemyPool[0];
            enemyPool.RemoveAt(0);
            float ps = UnityEngine.Random.Range(10, 20);
            node.speed = ps / 10;
            currEnemy.Add(node);
            return node;
        }
        GameObject go = GameObject.Instantiate(enemy_image);
        CollectMaterialEnemyNode grid = go.GetComponent<CollectMaterialEnemyNode>();
        grid.EnemyNodeEvent += OverGame;
        currEnemy.Add(grid);
        return grid;

    }
    private void PushEnemyItem(CollectMaterialEnemyNode node)
    {
        if (node.tween != null)
        {
            node.tween.Kill();
        }
        currEnemy.Remove(node);
        enemyPool.Add(node);
        node.gameObject.SetActive(false);

    }
    private void OverGame(Collider2D collision,CollectMaterialEnemyNode node)
    {
        Debug.Log(collision.name);
        if ("ball_shizhu_bones(Clone)" == collision.name)
        {
            DragonBonesController dragon = node.transform.Find("ball_bones").GetComponent<DragonBonesController>();
            string animName = "";
            if (node.direction == 0)
            {
                animName = "01_TurnDown";
            }
            else if (node.direction == 1)
            {
                animName = "02_TurnUp";

            }
            else if (node.direction == 2)
            {
                animName = "03_TurnRight";
            }
            else if (node.direction == 3)
            {
                animName = "04_TurnLeft";
            }
            if (node.tween != null)
            {
                node.tween.Kill();
            }
            dragon.PlayAnimation(animName, true);
            node.tween = node.transform.DOMove(grid2D[node.gridNode.gridZ, node.gridNode.gridI].transform.position, node.speed);
            node.tween.SetEase(Ease.Linear);
            node.tween.onComplete = () =>
            {
                EnemyTween(node);
            };
        }
        else if ("enemy_image(Clone)" == collision.name)
        {
            if (isOver == false)
            {
                DragonBonesController bomb = UiManager.LoadBonesByNmae("ball_bomb_bones");
                bomb.transform.SetParent(node.transform);
                bomb.transform.localPosition = Vector3.zero;
                bomb.transform.localScale = Vector3.one;
                for (int i = 0; i < currEnemy.Count; i++)
                {
                    if (currEnemy[i].tween != null)
                    {
                        currEnemy[i].tween.Kill();
                    }

                }
                proTween.Kill();
                bomb.PlayAnimation("01_Play", false, () =>
                {

                    Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(curr_diff), () =>
                    {
                        UpdataDiff(false);

                    }, () => { CloseSelf(); }, () =>
                    {
                        UpdataDiff();

                    });
                    GameObject.Destroy(bomb.gameObject);
                });

                isOver = true;
            }
           

        }
       
    }
}
