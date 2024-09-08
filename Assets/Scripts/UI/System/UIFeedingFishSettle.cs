using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFeedingFishSettle : UIBase
{
    public Button come_back_btn;
    public Button close_btn;
    private Action callBack;
    private Action exitCallBack;
    public Image resultImag;
    public RectTransform reward_item;
    public Button next_btn;
    public RectTransform item_root;
    private Action nextCallBack;
    public GameObject reward_icon;
    public override void OnAwake()
    {
       
    }
    //1成功 ，2//失败 //3 成功且没有下一个难度
    public void SetCallBack(int resultType, int dropId, Action callBack, Action exitCallBack, Action nextCallBack = null)
    {
        this.callBack = callBack;
        this.exitCallBack = exitCallBack;
        this.nextCallBack = nextCallBack;
        
        ShowItem(dropId, resultType);
        if (resultType == 1)
        {
            close_btn.gameObject.SetActive(false);
            resultImag.sprite = UiManager.LoadSprite("common", "success_word");
        }
        else if (resultType == 2)
        {
            next_btn.gameObject.SetActive(false);
            resultImag.sprite = UiManager.LoadSprite("common", "fail_word");
        }
        else if (resultType == 3)
        {
            next_btn.gameObject.SetActive(false);
            resultImag.sprite = UiManager.LoadSprite("common", "success_word");
        }
        resultImag.SetNativeSize();
    }
    private void ShowItem(int dropId, int resultType)
    {
        if (resultType == 2)
        {
            reward_icon.SetActive(false);
            return;
        }
        List<DropNode> itemIdList = DropManager.Instance.GetDropItemId2(dropId);
        if (itemIdList.Count == 0)
        {
            reward_icon.SetActive(false);

        }
        for (int i = 0; i < itemIdList.Count; i++)
        {
            GameObject go = GameObject.Instantiate(reward_item.gameObject);
            go.transform.SetParent(item_root);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            Image image = go.transform.Find("icon").GetComponent<Image>();
            TextMeshProUGUI num = go.transform.Find("num_").GetComponent<TextMeshProUGUI>();
            num.text ="x"+ itemIdList[i].count.ToString();
            if (itemIdList[i].itemId > 0)
            {
                itmeconfigData mCfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", itemIdList[i].itemId);
                image.sprite = UiManager.LoadSprite("item_icon", mCfg.icon);
            }
            else if (itemIdList[i].itemId == -2)
            {
                GameManage.userData.SetAddGoldValue(itemIdList[i].count);
                image.sprite = UiManager.LoadSprite("common", "gold_small_icon");
            }
            else if (itemIdList[i].itemId == -1)
            {
                GameManage.userData.SetAddExpValue(itemIdList[i].count);
                image.sprite = UiManager.LoadSprite("common", "exp_small_icon");
                
            }
        }

    }
    public override void OnStart()
    {
        come_back_btn.onClick.AddListener(OnClickCome);
        next_btn.onClick.AddListener(() =>
        {
            if (nextCallBack != null)
            {
                nextCallBack.Invoke();
                CloseSelf();
            }

        });

        close_btn.onClick.AddListener(() =>
        {
            CloseSelf();
            exitCallBack();
        });
    }
    private void OnClickCome()
    {


        CloseSelf();
        if (callBack != null)
        {
            callBack.Invoke();
        }




    }
}
