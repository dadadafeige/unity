using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class FeedingFishOneTimer
{
    public bool isTrue;
    public int fishId;
    public string m_name;

    // 创建 Random 对象
    System.Random random = new System.Random();
    public FeedingFishOneTimer()
    {
        // 生成随机数（包括2和4）
        fishId = random.Next(2, 6);
        Console.WriteLine("随机数: " + fishId);
        isTrue = random.Next(2) == 0;

        if (fishId == 2)
        {
            m_name = "蓝鲤鱼";
        }
        else if (fishId == 3)
        {
            m_name = "黄鲤鱼";
        }
        else if (fishId == 4)
        {
            m_name = "红鲤鱼";
        }
        else if (fishId == 5)
        {
            m_name = "绿鲤鱼";

        }

    }
    public bool CheckIsTrue(int fishId)
    {
        if (isTrue)
        {
            return this.fishId == fishId;
        }
        else
        {
            return this.fishId != fishId;
        }

    }
}
public class FeedingFishNode
{
    public itmeconfigData m_cfg;
    public int fishId;
    public string image_name;
    public FeedingFishNode(int fishId,string image_name)
    {

        this.fishId = fishId;
        this.image_name = image_name;
        m_cfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", fishId);

    }
}
public class UIFeedingFish : UIBase
{
    public Button close_btn;
    public TextMeshProUGUI right_num;
    public TextMeshProUGUI dialogue_label;
    public TextMeshProUGUI schedule;
    public RectTransform fish_group;
    public GameObject fish_item;
    private List<FeedingFishNode> fishNodeList = new List<FeedingFishNode>();
    public Image feedingfish_diff;
    public Image dialogue_bg;
    public Button click;
    public Image feedingfish_dialogue_head;
    public Image feedingfish_dialogue_pro;
    public Image result;
    public GameObject make;
    private List<GameObject> feedingObjList = new List<GameObject>();
    private int currDiff = 0; // 1 = 易  2 = 中 3 = 难
    private int condition; //答对 通关
    private int currPace; //当前进度
    private int maxPace; //每个难度的最大题目数量
    private int right_value; //每个难度的最大题目数量
    private FeedingFishOneTimer currOneTimer;
    private float fail_time;
    private Tween tween;
    private bool isPass = false;
    Action<bool> callBack;
    public Button rule_btn;
    public override void OnAwake()
    {
        fishNodeList.Add(new FeedingFishNode(2, "feedingfish_lanliyu"));
        fishNodeList.Add(new FeedingFishNode(3, "feedingfish_huangliyu"));
        fishNodeList.Add(new FeedingFishNode(4, "feedingfish_hongliyu"));
        fishNodeList.Add(new FeedingFishNode(5, "feedingfish_lvliyu"));

    }
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
        dialogue_label.text = "罗大人我今天饿的很，快给我奉上鲤鱼,(点击屏幕开始)";
        fish_group.gameObject.SetActive(false);
       // UpdataDiff();
        ShowLabel();
        click.onClick.AddListener(() =>
        {
            ShowUI();
            click.gameObject.SetActive(false);
            fish_group.gameObject.SetActive(true);
        });
        close_btn.onClick.AddListener(() =>
        {

            CloseSelf();
            callBack(isPass);
        });

    }
    public void SetCallBack(Action<bool> callBack)
    {

        this.callBack = callBack;

    }
    private void ReStart()
    {
        currDiff = 0;
        condition = 5; //答对 condition 通关
        currPace = 0; //当前进度
        right_value = 0;
        maxPace = 10; //每个难度的最大题目数量
        if (tween != null)
        {
            tween.Kill();
        }
        isPass = false;
        dialogue_label.text = "罗大人我今天饿的很，快给我奉上鲤鱼,(点击屏幕开始)";
        click.gameObject.SetActive(true);
        fish_group.gameObject.SetActive(false);
        UpdataDiff();
        ShowLabel();
    }
    private void ShowUI()
    {
        for (int i = 0; i < fishNodeList.Count; i++)
        {
            if (feedingObjList.Count <= i)
            {
                if (feedingObjList.Count == 0)
                {
                    feedingObjList.Add(fish_item);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(fish_item);
                    go.transform.SetParent(fish_group);
                    go.transform.localScale = Vector3.one;
                    feedingObjList.Add(go);
                }
            }
            ShowFishItem(fishNodeList[i], feedingObjList[i]);
        }
        UpdataTopic();
     

    }
    private void ShowLabel()
    {
        right_num.text = right_value.ToString();
        schedule.text = currPace + "/" + maxPace;
    }
    private bool UpdataDiff(bool isAdd = true)
    {
        //if (currPace == maxPace)
        //{
        //    if (right_value < condition)
        //    {

                

        //        return false;
        //    }
        //}
        if (isAdd)
        {
            currDiff += 1; // 1 = 易  2 = 中 3 = 难
        }
        condition = 5; //答对 condition 通关
        currPace = 0; //当前进度
        right_value = 0;
        maxPace = 10; //每个难度的最大题目数量
        feedingfish_diff.sprite = UiManager.LoadSprite("feedingfish", "feedingfish_diff" + currDiff);
        if (currDiff == 1)
        {
            fail_time = 3;
        }
        else if (currDiff == 2)
        {
            fail_time = 2;
        }
        else if (currDiff == 3)
        {
            fail_time = 1;
        }
        else
        {
            
         
            return false;
        }
        return true;


    }
    private void UpdataM(bool isAdd = true)
    {
        currOneTimer = new FeedingFishOneTimer();
        

        currPace++;
        feedingfish_dialogue_pro.fillAmount = 1;

        tween = feedingfish_dialogue_pro.DOFillAmount(0, fail_time);
        tween.onComplete = () =>
        {
            ShowResult(false);
        };
        dialogue_label.text = $"请给我奉上如下食物{currOneTimer.m_name}";
        if (currOneTimer.isTrue)
        {
            dialogue_bg.sprite = UiManager.LoadSprite("feedingfish", "feedingfish_dialogue_bg1");
            feedingfish_dialogue_head.sprite = UiManager.LoadSprite("feedingfish", "feedingfish_dialogue_head1");
        }
        else
        {
            dialogue_bg.sprite = UiManager.LoadSprite("feedingfish", "feedingfish_dialogue_bg2");
            feedingfish_dialogue_head.sprite = UiManager.LoadSprite("feedingfish", "feedingfish_dialogue_head2");
        }
        ShowLabel();
    }
    private void UpdataTopic()
    {
        if (currDiff == 1 && right_value == condition)
        {
            isPass = true;
        }
        if (right_value == condition || currPace == maxPace)
        {

            if (currPace == maxPace)
            {
                if (right_value < condition)
                {

                    Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(currDiff), () =>
                    {
                        bool isContinue = UpdataDiff(false);
                        if (!isContinue)
                        {
                            return;
                        }
                        UpdataM(false);

                    }, () => { CloseSelf(); callBack(isPass); }, () =>
                    {


                    });

                    return;
                }
            }
            if (currDiff == 3)
            {
                Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(currDiff), () =>
                {
                    bool isContinue = UpdataDiff(false);
                    if (!isContinue)
                    {
                        return;
                    }
                    UpdataM(false);

                }, () => { CloseSelf(); callBack(true); });
                return;
            }
            Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(currDiff), () =>
            {
                bool isContinue = UpdataDiff(false);
                if (!isContinue)
                {
                    return;
                }
                UpdataM(false);

            }, () => { }, () =>
            {
                bool isContinue = UpdataDiff();
                if (!isContinue)
                {
                    return;
                }
                UpdataM();

            });
            return;
        }
        UpdataM();

    }
    private void ShowFishItem(FeedingFishNode fishNode,GameObject go)
    {
        Image fish_item_icon = go.transform.Find("fish_item_icon").GetComponent<Image>();
        TextMeshProUGUI fish_item_name = go.transform.Find("fish_item_name").GetComponent<TextMeshProUGUI>();
        Button item_btn = go.transform.Find("item_btn").GetComponent<Button>();

        fish_item_icon.sprite = UiManager.LoadSprite("feedingfish", fishNode.image_name);
        fish_item_name.text = fishNode.m_cfg.name;
        item_btn.onClick.RemoveAllListeners();
        item_btn.onClick.AddListener(() =>{
            bool isSuccess = currOneTimer.CheckIsTrue(fishNode.fishId);
            ShowResult(isSuccess);
        });

    }
    private void ShowResult(bool isSuccess)
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        make.SetActive(true);
        if (isSuccess)
        {
            right_value++;
            result.sprite = UiManager.LoadSprite("feedingfish", "feedingfish_right1");
        }
        else
        {
            result.sprite = UiManager.LoadSprite("feedingfish", "feedingfish_wrong");
        }
        result.gameObject.SetActive(true);
        DelayedActionProvider.Instance.DelayedAction(() =>
        {
            result.gameObject.SetActive(false);
            UpdataTopic();
            make.SetActive(false);
        },0.5f);
        ShowLabel();
    }

}
