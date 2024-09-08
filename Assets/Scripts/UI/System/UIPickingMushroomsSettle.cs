using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPickingMushroomsSettle : UIBase
{

    public Image result;
    public Button mask;
    public GameObject itemObj;
    public RectTransform groupTran;
    public Button close_btn;
    public Button come_back_btn;
    public Button next_btn;
    Action action;
    Action closeAction;


    public override void OnAwake()
    {
      
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        next_btn.onClick.AddListener(() =>
        {
            action.Invoke();
            CloseSelf();

        });
        come_back_btn.onClick.AddListener(() =>
        {
            action.Invoke();
            CloseSelf();

        });
        close_btn.onClick.AddListener(() =>
        {
            closeAction.Invoke();
            CloseSelf();

        });

    }
    public void SetData(bool isSuccess, Action action,Action closeAction,Dictionary<int,int> itemIdMap)
    {
        this.action = action;
        this.closeAction = closeAction;
        foreach (KeyValuePair<int, int> item in itemIdMap)
        {
            GameObject go = GameObject.Instantiate(itemObj);
            go.transform.SetParent(groupTran);
            go.SetActive(true);
            go.transform.localScale = Vector3.one;
            Image icon = go.transform.Find("icon").GetComponent<Image>();
            TextMeshProUGUI num = go.transform.Find("num").GetComponent<TextMeshProUGUI>();
            itmeconfigData itmeconfig = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", item.Key);
            num.text = "X"+item.Value.ToString();
            icon.sprite = UiManager.LoadSprite("item_icon", itmeconfig.icon);
        }
        mask.onClick.AddListener(() =>
        {

            action();
            closeAction.Invoke();
            CloseSelf();

        });
        if (isSuccess)
        {
            result.sprite = UiManager.LoadSprite("common", "get_reward");
            come_back_btn.gameObject.SetActive(false);
            next_btn.gameObject.SetActive(true);
        }
        else
        {
            come_back_btn.gameObject.SetActive(true);
            next_btn.gameObject.SetActive(false);
            result.sprite = UiManager.LoadSprite("common", "fail_word");
        }
        result.SetNativeSize();
    }

}
