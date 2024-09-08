using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//public delegate void GemstoneEventHandler(int value,CollectMaterialGemstoneNode node);
//public delegate void EnemyEventHandler(Collider2D collision = null,CollectMaterialEnemyNode node = null);
public class UICollectMaterial2 : UIBase, IPointerDownHandler, IPointerUpHandler
{
    public CollectMaterialPlayerNode playerNode;
    private bool isMovingUp, isMovingDown, isMovingLeft, isMovingRight;
    // ��ȡ�ĸ���ť
    public EventTrigger upButton;
    public EventTrigger downButton;
    public EventTrigger leftButton;
    public EventTrigger rightButton;
    public RectTransform barricade_grid;
    public RectTransform enemy_root;
    public RectTransform grid_root;
    public GameObject barricade;
    public GameObject enemy_image;
    public GameObject gemstone_image;
    public GameObject grid_item;
    private CollectMaterialGridNode[,] grid2D = new CollectMaterialGridNode[7,5];
    private List<CollectMaterialGemstoneNode> gemstonePool = new List<CollectMaterialGemstoneNode>();
    private List<CollectMaterialEnemyNode> enemyPool = new List<CollectMaterialEnemyNode>();
    private List<CollectMaterialEnemyNode> currEnemyList = new List<CollectMaterialEnemyNode>();
    private List<CollectMaterialGemstoneNode> currGemstoneList = new List<CollectMaterialGemstoneNode>();
    public Image material_icon;
    public TextMeshProUGUI num_word;
    public TextMeshProUGUI material_icon_num;
    private int rewardNum;
    private int curNum;
    private int rewardId;
    private int enemyNum;
    private int diff = 0;
    public Button close_btn;
    public GameObject plate;
    public Button rule_btn;
    Rigidbody2D rb;

    public override void OnStart()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
         rb = playerNode.gameObject.GetComponent<Rigidbody2D>();
      
        rb.freezeRotation = true;
        AddEventTriggerdListener(upButton);
        AddEventTriggerdListener(downButton);
        AddEventTriggerdListener(leftButton);
        AddEventTriggerdListener(rightButton);
        InitBarricadeGrid();
        InitGrid();
        rule_btn.onClick.AddListener(() =>
        {
            for (int i = 0; i < currEnemyList.Count; i++)
            {
                if (currEnemyList[i].tween != null)
                {
                    currEnemyList[i].tween.Pause();
                }
            }
            MissionManage.ShowDescription(() =>
            {
                for (int i = 0; i < currEnemyList.Count; i++)
                {
                    if (currEnemyList[i].tween != null)
                    {
                        currEnemyList[i].tween.Play();
                    }
                }
            });



        });
        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () => {
                UpdataDiff();
            });
        });
       // UpdataDiff();

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            plate.SetActive(false);

        }


        stopwatch.Stop(); // ֹͣ��ʱ
        

        // ��ӡִ��ʱ��
        UnityEngine.Debug.Log($"ִ��ʱ��: {stopwatch.ElapsedMilliseconds} ����");
        close_btn.onClick.AddListener(CloseSelf);
    }
   
    private void InitBarricadeGrid()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int z = 0; z < 4; z++)
            {
                GameObject go = GameObject.Instantiate(barricade);
                go.transform.SetParent(barricade_grid);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                Image image = go.GetComponent<Image>();
                int randomNumber = UnityEngine.Random.Range(1, 6);
                image.sprite = UiManager.LoadSprite("collect_material2", "collect_material_barricade" + randomNumber);

            }
        }
    }
    private int time = 0;
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
            DelayedActionProvider.Instance.DelayedAction(() => { node.speed = node.speed - (node.speed * 0.2f); }, time / 2);
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
    private void EnemyTween(CollectMaterialEnemyNode node)
    {
        // ��ǰ�������
        int currentX = node.gridNode.gridZ;
        int currentY = node.gridNode.gridI;

        // ���ѡ��һ������0���ϣ�1���£�2����3���ң�
        System.Random rand = new System.Random();
        int direction = rand.Next(0, 4);

        // ������һ���������
        int nextX = currentX;
        int nextY = currentY;
        DragonBonesController dragon = node.transform.Find("collect_material_qiu").GetComponent<DragonBonesController>();
        string animName = "02_TurnUp";

        switch (direction)
        {

            case 0: // ��
                animName = "02_TurnUp";
                nextY--;
                break;
            case 1: // ��
                animName = "01_TurnDown";
                nextY++;
                break;
            case 2: // ��
                animName = "04_TurnLeft";
                nextX--;
                break;
            case 3: // ��
                animName = "03_TurnRight";
                nextX++;
                break;
        }
        // �����һ����������Ƿ��ڷ�Χ�ڣ�����ÿ��ֻ���ƶ�1��
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
            // ���ڷ�Χ�ڣ�����ѡ��һ���µķ��򲢼����µ���һ����
            do
            {
                direction = rand.Next(0, 4);

                nextX = currentX;
                nextY = currentY;
                switch (direction)
                {
                    case 0: // ��
                        animName = "02_TurnUp";
                        nextY--;
                        break;
                    case 1: // ��
                        animName = "01_TurnDown";
                        nextY++;
                        break;
                    case 2: // ��
                        animName = "04_TurnLeft";
                        nextX--;
                        break;
                    case 3: // ��
                        animName = "03_TurnRight";
                        nextX++;
                        break;
                }
            } while (!((nextX == currentX - 1 || nextX == currentX + 1 || nextX == currentX) &&
                       (nextY == currentY - 1 || nextY == currentY + 1 || nextY == currentY) &&
                       nextX >= 0 && nextX < grid2D.GetLength(0) && nextY >= 0 && nextY < grid2D.GetLength(1)));


            dragon.PlayAnimation(animName, true);
            // �������¼�������һ���������
            Console.WriteLine("���¼�������һ��������꣺(" + nextX + ", " + nextY + ")");
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
    private void GetReward(int itemId, CollectMaterialGemstoneNode node, Collider2D collision = null)
    {
        if (collision.name != "player")
        {
            return;
        }
        PushGemstoneItem(node);
        currGemstoneList.RemoveAt(0);
        curNum++;
        material_icon_num.text = "X" + curNum;
        if (curNum == rewardNum)
        {
            if (diff == 5)
            {
                for (int i = 0; i < currEnemyList.Count; i++)
                {
                    PushEnemyItem(currEnemyList[i]);
                }
                currEnemyList.Clear();
                Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(diff), () =>
                {
                    UpdatGemstoneItem();
                    UpdatEnemyItem();

                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });

                curNum = 0;
                material_icon_num.text = "X" + curNum;
                return;
            }
            else
            {
                for (int i = 0; i < currEnemyList.Count; i++)
                {
                    PushEnemyItem(currEnemyList[i]);
                }
                currEnemyList.Clear();
                Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(diff), () =>
                {
                    UpdatGemstoneItem();
                    UpdatEnemyItem();

                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });
            }
            //Common.Instance.ShowBones("nandutisheng_bones", () =>
            //{
            //});
            //UpdataDiff();
        }
     //   BagManage.Instance.Add(itemId);

    }
    private void UpdataDiff()
    {
      
        diff++;
        num_word.text = Common.Instance.SplitNumber(diff);
        curNum = 0;
        material_icon_num.text = "X" + curNum;
        if (diff == 1)
        {
            rewardId = 18;
            rewardNum = 1;
            enemyNum = 2;

        }
        else if (diff == 2)
        {
            rewardId = 14;
            rewardNum = 1;
            enemyNum = 3;
        }
        else if (diff == 3)
        {
            rewardId = 16;
            rewardNum = 1;
            enemyNum = 3;
        }
        else if (diff == 4)
        {
            rewardId = 15;
            rewardNum = 1;
            enemyNum = 4;
        }
        else if (diff == 5)
        {
            rewardId = 17;
            rewardNum = 1;
            enemyNum = 5;

        }
     
        itmeconfigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", rewardId);
        material_icon.sprite = UiManager.LoadSprite("item_icon", cfg.icon);
        UpdatGemstoneItem();
        UpdatEnemyItem();
    }
    private void UpdatGemstoneItem()
    {
        List<CollectMaterialGridNode> nodes = RandomUniqueValues(grid2D, rewardNum);
        for (int i = 0; i < currGemstoneList.Count; i++)
        {
            PushGemstoneItem(currGemstoneList[i]);
        }
        currGemstoneList.Clear();
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].gridType = 1;
            CollectMaterialGemstoneNode node = PopGemstoneItem();
            node.gameObject.transform.SetParent(nodes[i].transform);
            node.transform.localScale = Vector3.one;
            node.transform.localPosition = Vector3.zero;
            node.gameObject.SetActive(true);
            //node.UpdateGemstone(rewardId);
            currGemstoneList.Add(node);
        }
    }



    public static List<CollectMaterialGridNode> RandomUniqueValues(CollectMaterialGridNode[,] array, int N)
    {
        List<CollectMaterialGridNode> uniqueValues = new List<CollectMaterialGridNode>();
        System.Random rand = new System.Random();

        int totalElements = array.GetLength(0) * array.GetLength(1);

        if (N > totalElements)
        {
            Console.WriteLine("Ҫ����������������е�Ԫ��������");
            return uniqueValues;
        }

        while (uniqueValues.Count < N)
        {
            int randomIndex = rand.Next(totalElements);
            int row = randomIndex / array.GetLength(1);
            int col = randomIndex % array.GetLength(1);
            if (row > 1 && col >1)
            {
                CollectMaterialGridNode value = array[row, col];
                if (!uniqueValues.Contains(value))
                {
                    uniqueValues.Add(value);
                }
            }
        }

        return uniqueValues;
    }
    private void InitGrid()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int z = 0; z < 7; z++)
            {
                GameObject go = GameObject.Instantiate(grid_item);
                go.transform.SetParent(grid_root);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                CollectMaterialGridNode grid = go.GetComponent<CollectMaterialGridNode>();
                TextMeshProUGUI text = go.transform.Find("index").GetComponent<TextMeshProUGUI>();
                //text.text = z + "," + i;
                text.gameObject.SetActive(false);
                grid2D[z,i] = grid;
                grid.gridZ = z;
                grid.gridI = i;
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(grid_root);
    }
    private CollectMaterialGemstoneNode PopGemstoneItem()
    {
        if (gemstonePool.Count > 0)
        {
            CollectMaterialGemstoneNode node = gemstonePool[0];
            gemstonePool.RemoveAt(0);
            return node;
        }
        GameObject go = GameObject.Instantiate(gemstone_image);
        CollectMaterialGemstoneNode grid = go.GetComponent<CollectMaterialGemstoneNode>();
        grid.GemstoneNodeEvent += GetReward;
        return grid;
    }
    private void PushGemstoneItem(CollectMaterialGemstoneNode node)
    {
        gemstonePool.Add(node);
        node.gameObject.SetActive(false);
      
    }
    private void OverGame(Collider2D collision, CollectMaterialEnemyNode node)
    {
        if (collision.name != "player")
        {
            return;
        }
        for (int i = 0; i < currEnemyList.Count; i++)
        {
            PushEnemyItem(currEnemyList[i]);
        }
        currEnemyList.Clear();
        Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(diff), () =>
        {
            curNum = 0;
            material_icon_num.text = "X" + curNum;
            UpdatGemstoneItem();
            UpdatEnemyItem();

        }, () => { CloseSelf(); }, () =>
        {
            UpdataDiff();

        });

    }
    private CollectMaterialEnemyNode PopEnemyItem()
    {
        if (enemyPool.Count > 0)
        {
            CollectMaterialEnemyNode node = enemyPool[0];
            enemyPool.RemoveAt(0);
            float ps = UnityEngine.Random.Range(10, 20);
            node.speed = ps / 10;
            return node;
        }
        GameObject go = GameObject.Instantiate(enemy_image);
        CollectMaterialEnemyNode grid = go.GetComponent<CollectMaterialEnemyNode>();
        grid.EnemyNodeEvent += OverGame;
        return grid;

    }
    private void PushEnemyItem(CollectMaterialEnemyNode node)
    {
        if (node.tween != null)
        {
            node.tween.Kill();
        }
        enemyPool.Add(node);
        node.gameObject.SetActive(false);

    }
    private void AddEventTriggerdListener(EventTrigger trigger)
    {
        // �����갴���¼�������
        EventTrigger.Entry pressEntry = new EventTrigger.Entry();
        pressEntry.eventID = EventTriggerType.PointerDown;
        pressEntry.callback.AddListener((data) => { OnButtonPressed(data as PointerEventData); });
        trigger.triggers.Add(pressEntry);
        // ������̧���¼�������
        EventTrigger.Entry releaseEntry = new EventTrigger.Entry();
        releaseEntry.eventID = EventTriggerType.PointerUp;
        releaseEntry.callback.AddListener((data) => { OnButtonReleased(data as PointerEventData); });
        trigger.triggers.Add(releaseEntry);
    }
   
 
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMovingUp = isMovingDown = isMovingLeft = isMovingRight = false;
    }

    private void Update()
    {
       
        // ��ⰴ�����벢�ƶ���ɫ
        if (Input.GetKey(KeyCode.W))
        {
            playerNode.MoveUp();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerNode.MoveDown();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerNode.MoveLeft();

        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerNode.MoveRight();
        }
        
       
        if ( Input.GetMouseButtonUp(0))
        {
            OnPointerUp(null);
        }
        if (isMovingUp)
            playerNode.MoveUp();
        if (isMovingDown)
            playerNode.MoveDown();
        if (isMovingLeft)
            playerNode.MoveLeft();
        if (isMovingRight)
            playerNode.MoveRight();
        float moveDistance = rb.velocity.magnitude * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, rb.velocity.normalized, moveDistance);

        if (hit.collider != null)
        {
            // �����ϰ��������ײ
            rb.velocity = Vector2.zero;
            rb.position = hit.point;
        }


    }
    // ����ť������ʱ���õķ���
    void OnButtonPressed(PointerEventData eventData)
    {
        OnPointerUp(null);
        if (eventData.pointerCurrentRaycast.gameObject.name == "upButton")
            isMovingUp = true;
        else if (eventData.pointerCurrentRaycast.gameObject.name == "downButton")
            isMovingDown = true;
        else if (eventData.pointerCurrentRaycast.gameObject.name == "leftButton")
            isMovingLeft = true;
        else if (eventData.pointerCurrentRaycast.gameObject.name == "rightButton")
            isMovingRight = true;
    }

    // ����ť��̧��ʱ���õķ���
    void OnButtonReleased(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "UpButton")
            isMovingUp = false;
        else if (eventData.pointerCurrentRaycast.gameObject.name == "DownButton")
            isMovingDown = false;
        else if (eventData.pointerCurrentRaycast.gameObject.name == "LeftButton")
            isMovingLeft = false;
        else if (eventData.pointerCurrentRaycast.gameObject.name == "RightButton")
            isMovingRight = false;
    }

    // ֹͣ�ƶ�
    //public void StopMovement(BaseEventData eventData)
    //{
    //    direction = Vector3.zero;
    //}
}
