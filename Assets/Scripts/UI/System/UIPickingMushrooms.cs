using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class FeedingPickingOneTimer
{
    public int itemType;//1 == Ģ�� �ɼ� 2 == �ھ�  3 == ������
    public int itemId;
    // ���影��1���н�����
    float probabilityPool1 = 4 / 5.0f; // 4:1

    // ���影��2���н�����
    float probabilityPool2 = 1 / 5.0f; // 1:1

    // ���� Random ����
    System.Random random = new System.Random();
    public FeedingPickingOneTimer(List<int> itemPool, List<int> itemDirtyPool)
    { 
    // ����һ���������ֵ��0��1֮��ĸ�������
        float randomProbability = RandomFloat(); // ����� RandomFloat() ��ʾ��������������ĺ���
                                                 // �����������ֵȷ��ѡ���ĸ�����
        if (randomProbability < probabilityPool1)
        {
            // ����һ����8��10���������������8��10��
            int randomNumber = itemPool[RandomInteger(0, itemPool.Count - 1)];
         
            itemId = randomNumber;
          //  Debug.Log("�ӽ���1�г齱");
            if (itemId == 8)
            {
                itemType = 1;
            }
            else
            {
                itemType = 2;
            }
        }
        else
        {
            // �ӽ���2�齱
            // ���н���2�ĳ齱�߼�
            // ...
            itemType = 3;
            int randomNumber = itemDirtyPool[RandomInteger(0, itemDirtyPool.Count - 1)];
            itemId = randomNumber;
         //   Debug.Log("�ӽ���2�г齱");
        }

    }
    

    // ������������ĺ���
    int RandomInteger(int minValue, int maxValue)
    {
        return UnityEngine.Random.Range(minValue, maxValue + 1);
    }

    float RandomFloat()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }
    //1 ��  2��
    public bool CheckIsTrue(int btnType)
    {

        return itemType == btnType;
     
    }
}
public class UIPickingMushrooms : UIBase
{
    public Button picking_btn;
    public Button dig_out_btn;
    public TextMeshProUGUI round;
    public TextMeshProUGUI get_num;
    public TextMeshProUGUI item_name;
    public Image time_pro;
    public Image item_icon;
    public Image diff;
    public Image error_img;
    public GameObject mask2;
    public Button close_btn;
    public Button bag_btn;
    private int currDiff = 0; // 1 = ��  2 = �� 3 = ��
    private int currPace; //��ǰ����
    private int maxPace = 20; //ÿ���Ѷȵ������Ŀ����
    private float fail_time;
    private List<int> itemPool = new List<int>();
    private List<int> itemDirtyPool = new List<int>();
    FeedingPickingOneTimer oneTimer;
    private int right_num;
    private bool isMove = false;
    Tween tween;
    Dictionary<int, int> itemIdMap = new Dictionary<int, int>();
    public Button rule_btn;
    public override void OnAwake()
    {
      
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        //   mask.SetActive(false);
        close_btn.onClick.AddListener(CloseSelf);

        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                StartRound();
            });
        });

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
        bag_btn.onClick.AddListener(() =>
        {
            if (tween != null)
            {
                tween.Pause();

            }
            UIBag iBag = Common.Instance.ShowBag();
            iBag.SetDestroyCallback(() =>
            {
                if (tween != null)
                {
                    tween.Play();

                }

            });
        });
        picking_btn.onClick.AddListener(() =>{
            if (isMove == true)
            {
                return;
            }
            if (oneTimer != null)
            {
                if (tween != null)
                {
                    tween.Kill();
                    tween = null;
                }
                if (oneTimer.CheckIsTrue(1))
                {
                    if (!itemIdMap.ContainsKey(oneTimer.itemId))
                    {
                        itemIdMap.Add(oneTimer.itemId, 1);
                    }
                    else
                    {
                        itemIdMap[oneTimer.itemId]++;
                    }
                    mask2.SetActive(true);
                    UnityEngine.Vector3 tempos = item_icon.transform.position;
                    item_icon.transform.DOScale(UnityEngine.Vector3.zero, 0.5f);
                    isMove = true;
                    Tween moveTween = item_icon.transform.DOMove(bag_btn.transform.position, 0.5f);
                    moveTween.onComplete = () =>
                    {
                        mask2.SetActive(false);
                        item_icon.transform.position = tempos;
                        item_icon.transform.localScale = UnityEngine.Vector3.one;
                        CreateOneTimer();

                    };
                    BagManage.Instance.Add(oneTimer.itemId);
                    right_num++;
                    isMove = false;
                }
                else
                {
                    error_img.gameObject.SetActive(true);
                    mask2.SetActive(true);
                    DelayedActionProvider.Instance.DelayedAction(() =>
                    {
                        error_img.gameObject.SetActive(false);
                        mask2.SetActive(false);
                        CreateOneTimer();
                    }, 0.5f);
                }
            }

        });
        dig_out_btn.onClick.AddListener(() => {
            if (isMove == true)
            {
                return;
            }

            if (oneTimer != null)
            {

                if (tween != null)
                {
                    tween.Kill();
                    tween = null;
                }
                if (oneTimer.CheckIsTrue(2))
                {
                    UnityEngine.Vector3 tempos = item_icon.transform.position;
                    item_icon.transform.DOScale(UnityEngine.Vector3.zero, 0.5f);
                    isMove = true;
                    Tween moveTween = item_icon.transform.DOMove(bag_btn.transform.position, 0.5f);
                    mask2.SetActive(true);
                    if (!itemIdMap.ContainsKey(oneTimer.itemId))
                    {
                        itemIdMap.Add(oneTimer.itemId, 1);
                    }
                    else
                    {
                        itemIdMap[oneTimer.itemId]++;
                    }
                    moveTween.onComplete = () =>
                    {
                        isMove = false;
                        item_icon.transform.localScale = UnityEngine.Vector3.one;
                        mask2.SetActive(false);
                        item_icon.transform.position = tempos;
                        CreateOneTimer();

                    };
                    BagManage.Instance.Add(oneTimer.itemId);
                    right_num++;
                }
                else
                {
                    mask2.SetActive(true);
                    error_img.gameObject.SetActive(true);
                    DelayedActionProvider.Instance.DelayedAction(() =>
                    {
                        error_img.gameObject.SetActive(false);
                        mask2.SetActive(false);
                        CreateOneTimer();

                    }, 0.5f);
                }
            }
        });
   
    }
    public void Update()
    {
        // ��ⰴ�����벢�ƶ���ɫ
        if (Input.GetKeyUp(KeyCode.A))
        {
            picking_btn.onClick.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            dig_out_btn.onClick.Invoke();
        }
       
    }
    private void StartRound()
    {

        currDiff++;
        currPace = 0;
 

        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        if (currDiff == 1)
        {
            itemPool.Add(8);//itemId  Ģ��id
            itemDirtyPool.Add(11);//��Ģ��
            fail_time = 1f;
            maxPace = 10;
            diff.sprite = UiManager.LoadSprite("common", "diff_1");
            right_num = 0;
            itemIdMap.Clear();
            CreateOneTimer();

        }
        else if (currDiff == 2)
        {
            UIPickingMushroomsSettle gui = UiManager.OpenUI<UIPickingMushroomsSettle>("UIPickingMushroomsSettle");
            gui.SetData(right_num >= 10,
                () => {
                    if (right_num < 10)
                    {
                        currDiff = 1;
                        itemIdMap.Clear();
                        right_num = 0;
               
                        itemIdMap.Clear();
                        CreateOneTimer();
                        return;

                    }
                    itemPool.Add(9);//itemId  ��ҩ
                    itemDirtyPool.Add(12);//ľ��
                    fail_time = 0.9f;
                    maxPace = 15;
                    diff.sprite = UiManager.LoadSprite("common", "diff_3");
                    right_num = 0;
                    itemIdMap.Clear();
                    CreateOneTimer();
                },
                () => { CloseSelf(); },
                itemIdMap);
        
        }
        else if (currDiff == 3)
        {
          UIPickingMushroomsSettle gui = UiManager.OpenUI<UIPickingMushroomsSettle>("UIPickingMushroomsSettle");
          gui.SetData(right_num >= 10,
          () => {
              if (right_num < 10)
              {
                  currDiff = 2;
                  itemIdMap.Clear();
                  right_num = 0;
                  itemIdMap.Clear();
                  CreateOneTimer();
                  return;
              }
              itemPool.Add(10);//itemId  ��֥
              itemDirtyPool.Add(13);//�Ӳ�
              fail_time = 0.8f;
              maxPace = 20;
              diff.sprite = UiManager.LoadSprite("common", "diff_5");
              right_num = 0;
              itemIdMap.Clear();
              CreateOneTimer();
          },
          () => { CloseSelf(); },
          itemIdMap);
          
        }
        else
        {
            UIPickingMushroomsSettle gui = UiManager.OpenUI<UIPickingMushroomsSettle>("UIPickingMushroomsSettle");
            gui.SetData(right_num >= 10, 
                () => {
                    if (right_num < 10)
                    {
                        currDiff = 3;
                        itemIdMap.Clear();
                        right_num = 0;
                        itemIdMap.Clear();
                        CreateOneTimer();
                        return;
                    }
                }, 
                () => { CloseSelf(); },
                itemIdMap);
            if (right_num >= 10)
            {
                gui.come_back_btn.gameObject.SetActive(false);
                gui.next_btn.gameObject.SetActive(false);
            }
         
            // CloseSelf();
        }
    


    }
    private void CreateOneTimer()
    {
        currPace++;
        oneTimer = new FeedingPickingOneTimer(itemPool, itemDirtyPool);
        ShowUI();
    }
    private void ShowUI()
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        if (currPace > maxPace)
        {
            StartRound();
            return;
        }
        round.text = currPace + "/" + maxPace;
        itmeconfigData itmeconfig = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", oneTimer.itemId);
        item_icon.sprite = UiManager.LoadSprite("item_icon", itmeconfig.icon);
        
        time_pro.fillAmount = 0.91f;
        tween = time_pro.DOFillAmount(0.09f, fail_time);
        tween.onComplete = () =>
        {
            if (oneTimer.itemType == 3)
            {
                right_num++;
            }
            CreateOneTimer();

        };

    }
}
