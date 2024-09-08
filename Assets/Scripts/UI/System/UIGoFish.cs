using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIGoFish : UIBase
{

    public Image go_fish_pro;
    public Button go_fish_pro_btn;
    public Button go_fish_back_btn;
    public Button go_fish_bag_btn;
    public DragonBonesController dragon;
    public TextMeshProUGUI qiuyin_num;
    Tween tween;
    public Button rule_btn;
    public Image bg;
    public override void OnAwake()
    {
      
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        if (GameManage.curChapter == 5)
        {
            bg.sprite = UiManager.getTextureSpriteByNmae("go_fish2");
        }
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

        tween = go_fish_pro.DOFillAmount(0, 0.75f);
        tween.SetLoops(-1,LoopType.Yoyo);
        go_fish_pro_btn.onClick.AddListener(OnClickGoFishBtn);
        go_fish_back_btn.onClick.AddListener(CloseSelf);
        ItemNode item = BagManage.Instance.GetItemById(7);
        if (item != null)
        {
            qiuyin_num.text = "<sprite=10>" + Common.Instance.SplitNumber(item.count);
        }
        else
        {
            qiuyin_num.text = "<sprite=10>" + Common.Instance.SplitNumber(0);
        }
        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                
            });
        });
        go_fish_bag_btn.onClick.AddListener(() => { Common.Instance.ShowBag(); });
    }
    void OnClickGoFishBtn()
    {
        ItemNode item = BagManage.Instance.GetItemById(7);
        if (item == null || item.count <= 0)
        {
            Common.Instance.ShowTips("请去养殖场抓蚯蚓");
            return;
        }
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        BagManage.Instance.SubItemByItemAndNum(item, 1);
        if (item.count <= 0)
        {
            qiuyin_num.text = Common.Instance.SplitNumber(0);
        }
        else
        {
            qiuyin_num.text = Common.Instance.SplitNumber(item.count);
        }
       
        bool isSuccess = IsGetItem(go_fish_pro.fillAmount);
        int itemId = default;
        if (isSuccess)
        {
            itemId = DropManager.Instance.GetDropItemId(500);
            bool isAdd = BagManage.Instance.Add(itemId);
            if (!isAdd)
            {
                itemId = DropManager.Instance.GetDropItemId(500);
                BagManage.Instance.Add(itemId);
            }
        }
        dragon.PlayAnimation("02_DiaoYu", false, ()=>{
            dragon.PlayAnimation("01_Idle", true);
            if (isSuccess)
            {
                Common.Instance.ShowGetReward(go_fish_bag_btn.transform, itemId, () =>
                {
                    go_fish_pro.fillAmount = 1;
                    tween = go_fish_pro.DOFillAmount(0, 0.75f);
                    tween.SetLoops(-1, LoopType.Yoyo);
                });
            }
            else
            {
                go_fish_pro.fillAmount = 1;
                tween = go_fish_pro.DOFillAmount(0, 0.75f);
                tween.SetLoops(-1, LoopType.Yoyo);
                Common.Instance.ShowTips("钓鱼失败");

            }
        });
    }
    bool IsGetItem(float probability)
    {
        probability = probability * 10;
        // 生成0到99之间的随机数
        int randomValue = new System.Random().Next(10);

        // 如果随机数小于50，返回道具A，否则返回道具B
        return randomValue < probability;
    }


}
