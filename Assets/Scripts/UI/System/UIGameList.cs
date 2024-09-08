using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UIGameList : UIBase
{
    public GameObject itemItem;
    public RectTransform root;
    public Button close_btn;
    private List<MISSIONTYPE> uiNameList = new List<MISSIONTYPE>()
    {

        MISSIONTYPE.FLIPBRAND,
        MISSIONTYPE.GOFISH,
        MISSIONTYPE.FEEDINGFISH,
        MISSIONTYPE.PICKINGNMUSHROOMS,
        MISSIONTYPE.PUZZLE,
        MISSIONTYPE.LOG,
        MISSIONTYPE.FLANKER,
        MISSIONTYPE.COLLECTMATERIAL,
        MISSIONTYPE.GUESSINGPUZZLE,
        MISSIONTYPE.HURRYWAY,
        MISSIONTYPE.BALANCEBALL,
        MISSIONTYPE.RECALLWAY,
        MISSIONTYPE.GUESSINGFISTS,
        MISSIONTYPE.FINDSOMETHING,
        MISSIONTYPE.NUMBERMEMORY

};
    // Start is called before the first frame update
    public override void OnStart()
    {
        GameManage.LoadGame();
        for (int i = 0; i < GameManage.gameList.Count; i++)
        {
            GameObject go = GameObject.Instantiate(itemItem);
            go.transform.SetParent(root);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.SetActive(true);
            ShowItem(GameManage.gameList[i], go);
            //onClickAdd(btn, uiNameList[i]);
        }
        close_btn.onClick.AddListener(CloseSelf);


    }
    void ShowItem(GameNode node,GameObject go)
    {
        Button btn = go.GetComponent<Button>();
        TextMeshProUGUI textMesh = go.transform.Find("name").GetComponent<TextMeshProUGUI>();
        Image icon = go.transform.Find("icon").GetComponent<Image>();
        RectTransform reward_root = go.transform.Find("reward_root").GetComponent<RectTransform>();
        icon.sprite = UiManager.LoadSprite("game_list_icon", node.mCfg.game_icon);
        RectTransform reward_item = go.transform.Find("reward_root/reward_item").GetComponent<RectTransform>();
        textMesh.text = node.mCfg.name;
        string[] strs = node.mCfg.dropId.Split(",");
        int jinbiNum = 0;
        int jingyanNum = 0;
        for (int i = 0; i < strs.Length; i++)
        {
            List<DropNode> list = DropManager.Instance.GetDropItemId2(int.Parse(strs[i]));
            for (int z = 0; z < list.Count; z++)
            {
                if (list[z].itemId == -1)
                {
                    jingyanNum += list[z].count;
                }
                else if (list[z].itemId == -2)
                {
                    jinbiNum += list[z].count;
                }
            
            }
        }
        GameObject go1 = GameObject.Instantiate(reward_item.gameObject);
        go1.transform.SetParent(reward_root);
        go1.transform.localPosition = Vector3.zero;
        go1.transform.localScale = Vector3.one;
        go1.SetActive(true);
        Image reward_icon1 = go1.transform.Find("reward_icon").GetComponent<Image>();
        TextMeshProUGUI reward_num1 = go1.transform.Find("reward_num").GetComponent<TextMeshProUGUI>();
        reward_icon1.sprite = UiManager.LoadSprite("common", "exp_small_icon");
        reward_num1.text = jingyanNum.ToString();

        GameObject go2 = GameObject.Instantiate(reward_item.gameObject);
        go2.SetActive(true);
        go2.transform.SetParent(reward_root);
        go2.transform.localPosition = Vector3.zero;
        go2.transform.localScale = Vector3.one;
        Image reward_icon2 = go2.transform.Find("reward_icon").GetComponent<Image>();
        TextMeshProUGUI reward_num2 = go2.transform.Find("reward_num").GetComponent<TextMeshProUGUI>();
        reward_icon2.sprite = UiManager.LoadSprite("common", "gold_small_icon");
        reward_num2.text = jinbiNum.ToString();

        btn.onClick.AddListener(() =>
        {
            string pref_name = node.mCfg.prefab_name;
            GameManage.curGameMissionId = node.mCfg.mission_id;
            if (pref_name == "UIFindDifferen2")
            {
                pref_name = "UIFindDifferen";
            }
            Type type = Type.GetType(pref_name);
            if (type != null)
            {
                MethodInfo method = typeof(UiManager).GetMethod("OpenUI", BindingFlags.Static | BindingFlags.Public);
                MethodInfo genericMethod = method.MakeGenericMethod(type);
                genericMethod.Invoke(null, new object[] { node.mCfg.prefab_name });
            }
            else
            {
                Console.WriteLine("Invalid type name.");
            }
        });
        

    }
    public object CreateVariableOfType(string typeName, string value)
    {
        Type type = GetTypeFromName(typeName);
        if (type != null)
        {
            return ConvertToType(value, type);
        }
        return null;
    }

    public Type GetTypeFromName(string typeName)
    {
        switch (typeName)
        {
            case "UIFlipBrand":
                return typeof(UIFlipBrand);
            case "UIFeedingFish":
                return typeof(UIFeedingFish);
            case "UIPickingMushrooms":
                return typeof(UIPickingMushrooms);
            case "UIPuzzle":
                return typeof(UIPuzzle);
            case "UIFlanker":
                return typeof(UIFlanker);
            case "UICollectMaterial":
                return typeof(UICollectMaterial);
            case "UIGuessingPuzzle":
                return typeof(UIGuessingPuzzle);
            case "UIHurryWay":
                return typeof(UIHurryWay);
            case "UIBalanceBall":
                return typeof(UIBalanceBall);
            case "UIRecallWay":
                return typeof(UIRecallWay);
            case "UIGuessingFists":
                return typeof(UIGuessingFists);
            case "UINumbeRmemory":
                return typeof(UINumbeRmemory);
            case "UIFindSomething":
                return typeof(UIFindSomething);
            case "UISeabedPuzzle":
                return typeof(UISeabedPuzzle);
            case "UICollectMaterial2":
                return typeof(UICollectMaterial2);
            case "UIFindDifferen":
                return typeof(UIFindDifferen);
            case "UIShootingSun":
                return typeof(UIShootingSun);
            case "UIRecallWay2":
                return typeof(UIRecallWay2);
            case "UIPuzzle2":
                return typeof(UIPuzzle3);
            case "UIPuzzle3":
                return typeof(UIPuzzle2);
            case "UIFeedingFish2":
                return typeof(UIFeedingFish2);
            case "UIRecoveryHp":
                return typeof(UIRecoveryHp);
            case "UIFindDifferen2":
                return typeof(UIFindDifferen);
            // 可以根据需要添加更多类型
            default:
                return null;
        }
    }
    public static object ConvertToType(string value, Type type)
    {
        try
        {
            return Convert.ChangeType(value, type);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to convert value: {ex.Message}");
            return null;
        }
    }
    public override void OnDestroyImp()
    {
        GameManage.curGameMissionId = GameManage.curMissionId;
    }

    //void onClickAdd(Button btn, MISSIONTYPE type )
    //{
    //        TextMeshProUGUI text = btn.transform.Find("name").GetComponent<TextMeshProUGUI>();
    //        text.text = "Ϸ" + (int)type;
    //        btn.onClick.AddListener(() =>
    //        {

    //         if (type == MISSIONTYPE.FLIPBRAND)
    //            {
    //                UIFlipBrand uILogon = UiManager.OpenUI<UIFlipBrand>("UIFlipBrand");
    //                uILogon.SetCallBack(() =>
    //                {

    //                });

    //            }
    //            else if (type == MISSIONTYPE.GOFISH)
    //            {
    //                UiManager.OpenUI<UIGoFish>("UIGoFish");

    //            }
    //            else if (type == MISSIONTYPE.BATTLE)
    //            {
    //                BattleManager.Instance.StartBattle(1);

    //            }
    //            else if (type == MISSIONTYPE.FEEDINGFISH)
    //            {


    //                UIFeedingFish mui = UiManager.OpenUI<UIFeedingFish>("UIFeedingFish");
    //                mui.SetCallBack((isSuccess) => {



    //                });

    //            }
    //            else if (type == MISSIONTYPE.PICKINGNMUSHROOMS)
    //            {
    //                UiManager.OpenUI<UIPickingMushrooms>("UIPickingMushrooms");

    //            }
    //            else if (type == MISSIONTYPE.PUZZLE)
    //            {

    //                UIPuzzle uIPuzzle = UiManager.OpenUI<UIPuzzle>("UIPuzzle");
    //                uIPuzzle.SetCallBack(() => {



    //                });

    //            }
    //            else if (type == MISSIONTYPE.LOG)
    //            {
    //                UiManager.OpenUI<UIMainLog>("UIMainLog");

    //            }
    //            else if (type == MISSIONTYPE.FLANKER)
    //            {
    //                UiManager.OpenUI<UIFlanker>("UIFlanker");

    //            }
    //            else if (type == MISSIONTYPE.COLLECTMATERIAL)
    //            {
    //                UiManager.OpenUI<UICollectMaterial>("UICollectMaterial");

    //            }
    //            else if (type == MISSIONTYPE.GUESSINGPUZZLE)
    //            {
    //                UiManager.OpenUI<UIGuessingPuzzle>("UIGuessingPuzzle");

    //            }
    //            else if (type == MISSIONTYPE.HURRYWAY)
    //            {
    //                UiManager.OpenUI<UIHurryWay>("UIHurryWay");

    //            }
    //            else if (type == MISSIONTYPE.BALANCEBALL)
    //            {
    //                UiManager.OpenUI<UIBalanceBall>("UIBalanceBall");

    //            }
    //            else if (type == MISSIONTYPE.RECALLWAY)
    //            {
    //                UIRecallWay mui = UiManager.OpenUI<UIRecallWay>("UIRecallWay");


    //            }
    //            else if (type == MISSIONTYPE.GUESSINGFISTS)
    //            {
    //                UIGuessingFists mui = UiManager.OpenUI<UIGuessingFists>("UIGuessingFists");

    //            }else if(type == MISSIONTYPE.FINDSOMETHING)
    //            {
    //                UIFindSomething mui = UiManager.OpenUI<UIFindSomething>("UIFindSomething");
    //            }else if(type == MISSIONTYPE.NUMBERMEMORY) 
    //            {
    //                UINumbeRmemory mui = UiManager.OpenUI<UINumbeRmemory>("UINumbeRmemory");
    //            }

    //        });

    //    }


    // Update is called once per frame

}
