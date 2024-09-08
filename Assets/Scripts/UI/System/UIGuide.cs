using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LoopScrollView;


public class UIGuide : UIBase
{
    public LoopScrollView loopScrollView;
    public Button close_btn;
    private List<skillcnofigData> skills = new List<skillcnofigData>();
    // Start is called before the first frame update
    public override void OnStart()
    {
        // 设置回调方法
        loopScrollView.F_SetOnItemLoadHandler(OnItemLoad);
        // 初始化并设置元素个数
        loopScrollView.F_Init();
        InitData();
        close_btn.onClick.AddListener(CloseSelf);


    }
    private void InitData()
    {
        Dictionary<int, skillcnofigData> itemCfg = GetCfgManage.Instance.GetCfgByName<skillcnofigData>("skill");
        foreach (skillcnofigData item in itemCfg.Values)
        {
            skills.Add(item);
        }
        loopScrollView.F_SetItemCount(skills.Count); // 设置列表项的总数

    }
    private void OnItemLoad(GameObject obj, int index)
    {
        Image name_ = obj.transform.Find("name").GetComponent<Image>();
        TextMeshProUGUI explain = obj.transform.Find("explain").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI limit = obj.transform.Find("limit").GetComponent<TextMeshProUGUI>();
        Image type_icon = obj.transform.Find("type_icon").GetComponent<Image>();
        Image draw = obj.transform.Find("draw").GetComponent<Image>();
        Transform guide7 = obj.transform.Find("guide7");
        Transform transparent = obj.transform.Find("transparent");
        Transform guide8 = obj.transform.Find("guide8");
        skillcnofigData cfg = skills[index];
        player_attributecnofigData player_ = GetCfgManage.Instance.GetCfgByNameAndId<player_attributecnofigData>("player_attribute", cfg.level_limit);
        name_.sprite = UiManager.LoadSprite("guide_name", cfg.name_image);
        draw.sprite = UiManager.LoadSprite("guide_name", cfg.image);
        transparent.gameObject.SetActive(index % 2 == 0);


        explain.text = cfg.explain;
        limit.text = player_.title;
        guide7.gameObject.SetActive(cfg.level_limit <= GameManage.userData.level);
        draw.gameObject.SetActive(cfg.level_limit <= GameManage.userData.level);
        guide8.gameObject.SetActive(cfg.level_limit > GameManage.userData.level);
        if (cfg.type != 1)
        {
            type_icon.sprite = UiManager.LoadSprite("guide", "guide4");
        }
        else
        {
            type_icon.sprite = UiManager.LoadSprite("guide", "guide5");
        }

    }
}
