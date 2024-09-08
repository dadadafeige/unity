using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static MissionManage;

[Serializable]
public class MissionNode
{
   //[NonSerialized]
    public missioncnofigData mCfg;
    public MISSIONSTATE missionState = MISSIONSTATE.RECEIVE;
    Dictionary<MISSIONSTATE, List<PlotNode>> plotListMap = new Dictionary<MISSIONSTATE, List<PlotNode>>();
    public SceneNode sceneNode;
    public PlotNode curPlot;
    public int rewardNum;
    public bool isNewBie = false;

    public MissionNode(missioncnofigData missionCfg)
    {

        mCfg = missionCfg;
        Debug.Log(missionCfg.sceneId);
        scenecnofigData sceneCfg = GetCfgManage.Instance.GetCfgByNameAndId<scenecnofigData>("scene", missionCfg.sceneId);
        sceneNode = new SceneNode(sceneCfg);

    }
    public List<PlotNode> GetPlotList()
    {
        if (plotListMap.ContainsKey(missionState))
        {
            return plotListMap[missionState];
        }
        string plotStr = "";
        if (missionState == MISSIONSTATE.RECEIVE)
        {
            plotStr = mCfg.receivePlot;
        }
        else if (missionState == MISSIONSTATE.UNFINISH)
        {
            plotStr = mCfg.unfinishPlot;
        }
        else if (missionState == MISSIONSTATE.FINISH)
        {
            plotStr = mCfg.finishPlot;
        }
        else
        {
            if (GameManage.isMap)
            {
                plotStr = mCfg.dailyPlot;
            }
            else
            {
                plotStr = mCfg.receivePlot;

            }
           

        }
        List<PlotNode> plots = new List<PlotNode>();
        if (plotStr != "")
        {
            string[] strArr = plotStr.Split(",");
            string cfgName = strArr[0];
            int startIndex = int.Parse(strArr[1]);
            int endIndex = int.Parse(strArr[2]);
            if (endIndex > 0)
            {
                PlotNode temPlot = null;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    try {

                        plotconfigData plotCfg = GetCfgManage.Instance.GetCfgByNameAndId<plotconfigData>(cfgName, i);
                        PlotNode plotNode = new PlotNode(plotCfg, this);
                        if (temPlot != null)
                        {
                            temPlot.SetNextPlot(plotNode);
                        }
                        temPlot = plotNode;
                        plots.Add(plotNode);
                    } 
                    catch
                    {
                        //更新mcfg
                        mCfg = GetCfgManage.Instance.GetCfgByNameAndId<missioncnofigData>("mission", mCfg.id);
                        return GetPlotList();
                    }
                    

                }
            }
            else
            {
                plotconfigData plotCfg = GetCfgManage.Instance.GetCfgByNameAndId<plotconfigData>(cfgName, startIndex);
                PlotNode plotNode = new PlotNode(plotCfg, this);
                plots.Add(plotNode);
            }
        }
        plotListMap.Add(missionState, plots);
        return plots;


    }
    private bool IsSuccess(PointNode[] pointArr)
    {
        string[] strArr = mCfg.param.Split(",");
        int len = 0;
        for (int i = 0; i < pointArr.Length; i++)
        {
            if (pointArr[i] != null)
            {
                len++;
            }
        }
        if (strArr.Length != len)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < len; i++)
            {
                if (pointArr[i].index + 1 != int.Parse(strArr[i]))
                {
                    return false;
                }
            }

            return true;
        }

    }
    private void SetStateFinish()
    {
        if (missionState == MISSIONSTATE.RECEIVE || missionState ==  MISSIONSTATE.UNFINISH)
        {
            missionState = MISSIONSTATE.FINISH;
            MissionManage.SaveItems();
        }

    }
    public void EnterWork(Action callBack,UIPlayerStory gui = null)
    {
        if (mCfg.type == (int)MISSIONTYPE.PURE_TALE)
        {
            SetStateFinish();
            callBack();
        }
        else if (mCfg.type == (int)MISSIONTYPE.DRAW_RUNE)
        {
            gui.OutUI();
            UILogon uILogon = UiManager.OpenUI<UILogon>("UILogon");
            List<int> ints = new List<int>();

            if (mCfg.param == "")
            {
                uILogon.new_bie.gameObject.SetActive(false);
                uILogon.isSelf = true;
                uILogon.SetData(this, (pointArr) =>
                {
                    int len = 0;
                    for (int i = 0; i < pointArr.Length; i++)
                    {
                        if (pointArr[i] != null)
                        {
                            len++;
                        }
                    }
                    Dictionary<int, skillcnofigData> dataMap = GetCfgManage.Instance.GetCfgByName<skillcnofigData>("skill");
                    string str = "";
                    for (int i = 0; i < len; i++)
                    {
                        if (str == "")
                        {
                            str = (pointArr[i].index + 1).ToString();

                        }
                        else
                        {
                            str = str + "," + (pointArr[i].index + 1).ToString();
                        }
                       

                    }
                    foreach (KeyValuePair<int,skillcnofigData >  item in dataMap)
                    {
                        if (item.Value.target == str)
                        {
                            uILogon.mask.SetActive(true);
                            skillcnofigData skillcfg = GetCfgManage.Instance.GetCfgByNameAndId<skillcnofigData>("skill", item.Key);
                            DragonBonesController wordBones = UiManager.LoadBonesByNmae("fuwenzi_bones");
                            wordBones.transform.SetParent(uILogon.word_root);
                            wordBones.transform.localPosition = Vector3.zero;
                            wordBones.transform.localScale = Vector3.one;
                            uILogon.mask.SetActive(true);
                            wordBones.PlayAnimation(skillcfg.effect_name, false, () =>
                            {
                                uILogon.bonesController.SwitchAndPlayAnimation("Attack", false, () =>
                                {
                                    uILogon.mask.SetActive(false);
                                    wordBones.gameObject.SetActive(false);
                                    ShowReward();
                                    SetStateFinish();
                                    callBack();
                                    uILogon.CloseSelf();

                                });
                            });
                            return;
                        }
                        
                    }
                    uILogon.CloseSelf();
                    if (missionState != MISSIONSTATE.DAILY)
                    {
                        missionState = MISSIONSTATE.UNFINISH;
                    }
           
                    MissionManage.SaveItems();
                    callBack();

                });
             
                return;
            }
          


            string[] strArr = mCfg.param.Split(",");
            int skillId = 1;
            string[] str = mCfg.endParam.Split(",");
            if (str[0] == "thief")
            {
                skillId = 13;

            }
            else if ("ShowBones" == mCfg.endParam)
            {
                skillId = 12;
            }
            else if ("QuGuiFu" == mCfg.endParam)
            {
                skillId = 14;
            }
            else if ("NiZongFu" == mCfg.endParam)
            {
                skillId = 15;
            }
            else if ("HuoShenFu" == mCfg.endParam)
            {
                skillId = 16;
            }
            else if ("ShenDunFu" == mCfg.endParam)
            {
                skillId = 17;
            }
            else if ("XianYinFu" == mCfg.endParam)
            {
                skillId = 15;
            }
            
            for (int i = 0; i < strArr.Length; i++)
            {
                ints.Add(int.Parse(strArr[i]) - 1);
            }


            if ("ShowBones" == mCfg.endParam)
            {
                uILogon.SetnewBieDotList(ints, () =>
                {

                    UIShowBones uIShow = UiManager.OpenUI<UIShowBones>("UIShowBones");
                    uIShow.PlayBones("shenzhifu_bones", () => {
                        SetStateFinish();
                        StartShake(gui.mSpineRoot[0]);
                        ShowReward();
                        callBack();

                    });
                

                });
                return;
            }
            else
            {
                uILogon.SetnewBieDotList(ints);
            }
            uILogon.SetIsSuccess(IsSuccess);
            uILogon.SetData(this, (pointArr) =>
            {
                int len = 0;
                for (int i = 0; i < pointArr.Length; i++)
                {
                    if (pointArr[i] != null)
                    {
                        len++;
                    }
                }

                string[] strArr = mCfg.param.Split(",");
                if (strArr.Length != len)
                {
                    gui.GoInUI();
                    if (missionState != MISSIONSTATE.DAILY)
                    {
                        missionState = MISSIONSTATE.UNFINISH;
                    }
              
                    MissionManage.SaveItems();
                }
                else
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (pointArr[i].index + 1 != int.Parse(strArr[i]))
                        {
                            if (missionState != MISSIONSTATE.DAILY)
                            {
                                missionState = MISSIONSTATE.UNFINISH;
                            }
                            MissionManage.SaveItems();
                            callBack();
                            gui.GoInUI();
                            return;
                        }
                    }
                    ShowReward();
                    if (str[0] != "smith")
                    {
                        Sprite sprite = UiManager.LoadSprite("common", "gold_small_icon");
                        Common.Instance.ShowGetReward(gui.top_item.gold.transform, sprite, () =>
                        {
                            SetStateFinish();
                            callBack();
                            gui.top_item.gold.gameObject.SetActive(true);

                        });
                        SetStateFinish();
                        callBack();
                    }
                   
                    if (gui != null)
                    {
                        if (mCfg.endParam != "QuGuiFu" && mCfg.endParam != "NiZongFu" && mCfg.endParam != "HuoShenFu"
                        && mCfg.endParam != "XianYinFu" && mCfg.endParam != "ShenDunFu" && mCfg.endParam != "HuoShenFu")
                        {
                            string[] str = mCfg.endParam.Split(",");
                            GameObject go = GameObject.Find(str[0]);
                            if (go != null)
                            {
                                DragonBonesController dragon = go.GetComponent<DragonBonesController>();
                                if (dragon != null)
                                {
                                    dragon.PlayAnimation(str[1], false, () =>
                                    {
                                        if (str.Length > 2)
                                        {
                                            dragon.PlayAnimation(str[2], true);
                                        }
                                        else
                                        {
                                            dragon.PlayAnimation(dragon.armatureComponent.animation.animationNames[0], true);
                                        }

                                    });
                                }
                            }
                            if (str[0] == "smith")
                            {
                                Sprite sprite = UiManager.LoadSprite("common", "gold_small_icon");
                                Common.Instance.ShowGetReward(gui.top_item.gold.transform, sprite, () =>
                                {
                                    SetStateFinish();
                                    callBack();
                                    gui.top_item.gold.gameObject.SetActive(true);

                                });
                            }
                        }
                    }
                 

                }
        
            }, skillId);
            if (mCfg.hangPos != "")
            {
                Transform tra = GameObject.Find(mCfg.hangPos).transform;
                uILogon.SetHangPoint(tra);

            }
       
            
        }
        else if (mCfg.type == (int)MISSIONTYPE.FLIPBRAND) ////
        {
            UIFlipBrand uILogon = UiManager.OpenUI<UIFlipBrand>("UIFlipBrand");
            uILogon.SetCallBack(() =>
            {
                Sprite sprite = UiManager.LoadSprite("common", "daily_btn");
                if (gui.daily_btn.gameObject.activeInHierarchy == false)
                {
                    Common.Instance.ShowGetReward(gui.daily_btn.transform, sprite, () =>
                    {
                        SetStateFinish();
                        callBack();
                        gui.daily_btn.gameObject.SetActive(true);

                    });
                }
               
         
            });

        }
        else if (mCfg.type == (int)MISSIONTYPE.GETREWARD)
        {
            if (mCfg.id == 14)
            {
                Sprite sprite = UiManager.LoadSprite("common", "draw_btn");
                if (gui.draw_btn.gameObject.activeInHierarchy == false)
                {
                    Common.Instance.ShowGetReward(gui.draw_btn.transform, sprite, () =>
                    {
                        SetStateFinish();
                        callBack();
                        gui.draw_btn.gameObject.SetActive(true);

                    });
                }
               
                return;
            }
            if (mCfg.reward != null && mCfg.reward != "")
            {
                string[] arr = mCfg.reward.Split(",");
                Common.Instance.ShowGetReward(gui.bag_btn.transform,int.Parse(arr[0]) , () =>
                {
                    SetStateFinish();
                    callBack();
                    gui.bag_btn.gameObject.SetActive(true);

                });
            }
            else
            {
                Common.Instance.ShowGetReward(gui.bag_btn.transform, () =>
                {
                    SetStateFinish();
                    callBack();
                    gui.bag_btn.gameObject.SetActive(true);

                });
            }
           
        }
        else if (mCfg.type == (int)MISSIONTYPE.GOFISH)//
        {
            UIGoFish uIGo = UiManager.OpenUI<UIGoFish>("UIGoFish");
            uIGo.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
            
        }
        else if (mCfg.type == (int)MISSIONTYPE.BATTLE)
        {
            BattleManager.Instance.StartBattle(int.Parse(mCfg.endParam) );
            BattleManager.Instance.uIBattle.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
          
        }
        else if (mCfg.type == (int)MISSIONTYPE.FEEDINGFISH)//
        {
            ItemNode lan =  BagManage.Instance.GetItemById(2);
            ItemNode huang = BagManage.Instance.GetItemById(3);
            ItemNode hong = BagManage.Instance.GetItemById(4);
            ItemNode lv = BagManage.Instance.GetItemById(5);
            if ((lan == null || lan.count <= 0) || (huang == null || huang.count<=0 )
                || (hong == null || hong.count <= 0) || (lv == null || lv.count <= 0))
            {
                Common.Instance.ShowTips("请前往鱼塘钓鱼，需要: 蓝鲤鱼、黄鲤鱼、红鲤鱼、绿鲤鱼 各一条");
                return;
            }
            BagManage.Instance.SubItemByItemAndNum(lan,1);
            BagManage.Instance.SubItemByItemAndNum(huang, 1);
            BagManage.Instance.SubItemByItemAndNum(hong, 1);
            BagManage.Instance.SubItemByItemAndNum(lv, 1);
            UIFeedingFish mui  = UiManager.OpenUI<UIFeedingFish>("UIFeedingFish");
            mui.SetCallBack((isSuccess) => {

                if (isSuccess == true)
                {
                    SetStateFinish();
                    callBack();
                   
                }
            
            });

        }
        else if (mCfg.type == (int)MISSIONTYPE.PICKINGNMUSHROOMS)    ////////////////暂时搁置
        {
            UIPickingMushrooms uIPicking = UiManager.OpenUI<UIPickingMushrooms>("UIPickingMushrooms");
            uIPicking.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();

            });
           
        }
        else if (mCfg.type == (int)MISSIONTYPE.PUZZLE)//
        {

            UIPuzzle uIPuzzle =  UiManager.OpenUI<UIPuzzle>("UIPuzzle");
            uIPuzzle.SetCallBack(() => {

                SetStateFinish();
                callBack();

            });

        }
        else if (mCfg.type == (int)MISSIONTYPE.LOG)
        {
            if (GameManage.userData.gold > 100)
            {
                GameManage.userData.SetAddGoldValue(-100);
            }
            else
            {
                Common.Instance.ShowTips("金币不足，请前往行囊出售药材");
                if (missionState != MISSIONSTATE.DAILY)
                {
                    missionState = MISSIONSTATE.UNFINISH;
                }
                MissionManage.SaveItems();
                //    callBack();
                return;
            }
            UIMainLog uIMain = UiManager.OpenUI<UIMainLog>("UIMainLog");
            if (!isNewBie)
            {
                isNewBie = true;
                uIMain.OpenNewBie();
            }
        
            uIMain.SetDestroyCallback(() =>
            {
                Sprite sprite = UiManager.LoadSprite("common", "logs_btn");
                if (missionState != MISSIONSTATE.DAILY)
                {
                    Common.Instance.ShowGetReward(gui.logs_btn.transform, sprite, () =>
                    {
                        SetStateFinish();
                        callBack();
                        gui.logs_btn.gameObject.SetActive(true);

                    });
                }
                else
                {
                    SetStateFinish();
                    callBack();
                    gui.logs_btn.gameObject.SetActive(true);
                }
               
       
            });
          
        }
        else if (mCfg.type == (int)MISSIONTYPE.FLANKER)///.
        {
            UIFlanker iFlanker = UiManager.OpenUI<UIFlanker>("UIFlanker");
            iFlanker.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();

            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.COLLECTMATERIAL)//
        {
            UICollectMaterial uICollect = UiManager.OpenUI<UICollectMaterial>("UICollectMaterial");
            uICollect.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
   
        }
        else if (mCfg.type == (int)MISSIONTYPE.GUESSINGPUZZLE)//
        {
            UIGuessingPuzzle uIGuessing = UiManager.OpenUI<UIGuessingPuzzle>("UIGuessingPuzzle");
            uIGuessing.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
      
        }
        else if (mCfg.type == (int)MISSIONTYPE.LONGTIME)
        {
            UILongTime uILong = UiManager.OpenUI<UILongTime>("UILongTime");
            uILong.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();

            });
        
        }
        else if (mCfg.type == (int)MISSIONTYPE.HURRYWAY)//
        {
            UIHurryWay uIHurry = UiManager.OpenUI<UIHurryWay>("UIHurryWay");
            uIHurry.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
  
        }
        else if (mCfg.type == (int)MISSIONTYPE.BALANCEBALL)//
        {
            UIBalanceBall uIBalance = UiManager.OpenUI<UIBalanceBall>("UIBalanceBall");
            uIBalance.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.RECALLWAY)//
        {
            UIRecallWay mui = UiManager.OpenUI<UIRecallWay>("UIRecallWay");
            mui.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });

        }
        else if (mCfg.type == (int)MISSIONTYPE.GUESSINGFISTS)//
        {
            UIGuessingFists mui = UiManager.OpenUI<UIGuessingFists>("UIGuessingFists");
            mui.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.GETPET)
        {
       
            Sprite sprite = UiManager.getTextureSpriteByNmae("pet_reward");
            Common.Instance.ShowGetReward(gui.role_btn.transform, sprite, () =>
            {
                SetStateFinish();
                callBack();
                gui.role_btn.gameObject.SetActive(true);

            });

        }
        else if (mCfg.type == (int)MISSIONTYPE.NUMBERMEMORY)//
        {
            UINumbeRmemory mui = UiManager.OpenUI<UINumbeRmemory>("UINumbeRmemory");
            mui.SetIsNewBie(true);
            mui.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });

        }
        else if (mCfg.type == (int)MISSIONTYPE.FINDSOMETHING)//
        {
            UIFindSomething mui = UiManager.OpenUI<UIFindSomething>("UIFindSomething");
            mui.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.SEABEDPUZZLE)//
        {
            UISeabedPuzzle mui = UiManager.OpenUI<UISeabedPuzzle>("UISeabedPuzzle");
            mui.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.COLLECTMATERIAL2)//
        {
            UICollectMaterial2 uICollect = UiManager.OpenUI<UICollectMaterial2>("UICollectMaterial2");
            uICollect.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.SHOOTINGSUN)//
        {
            UIShootingSun shootingSun  = UiManager.OpenUI<UIShootingSun>("UIShootingSun");

            shootingSun.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.FINDDIFFEREN)//
        {
            UIFindDifferen findDifferen = UiManager.OpenUI<UIFindDifferen>("UIFindDifferen");

            findDifferen.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.PUZZLE2)//
        {
            UIPuzzle2 puzzle2 = UiManager.OpenUI<UIPuzzle2>("UIPuzzle2");

            puzzle2.SetDestroyCallback(() =>
            {
                BagManage.Instance.SubItemByItemAndNum(20, 1);
                BagManage.Instance.SubItemByItemAndNum(21, 1);
                BagManage.Instance.SubItemByItemAndNum(22, 1);
                BagManage.Instance.SubItemByItemAndNum(23, 1);
                BagManage.Instance.SubItemByItemAndNum(24, 1);
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.RECALLWAY2)//
        {
            UIRecallWay2 recallWay2 = UiManager.OpenUI<UIRecallWay2>("UIRecallWay2");

            recallWay2.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.RECOVERYHP)//
        {
            UIRecoveryHp recovery = UiManager.OpenUI<UIRecoveryHp>("UIRecoveryHp");

            recovery.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.FINDDIFFEREN2)
        {
            UIFindDifferen recovery = UiManager.OpenUI<UIFindDifferen>("UIFindDifferen2");

            recovery.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.FEEDINGFISH2)
        {   
            UIFeedingFish2 recovery = UiManager.OpenUI<UIFeedingFish2>("UIFeedingFish2");

            recovery.SetDestroyCallback(() =>
            {
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.PUZZLE3)
        {
            UIPuzzle3 recovery = UiManager.OpenUI<UIPuzzle3>("UIPuzzle3");
            recovery.SetDestroyCallback(() =>
            {
                BagManage.Instance.SubItemByItemAndNum(25, 1);
                BagManage.Instance.SubItemByItemAndNum(26, 1);
                BagManage.Instance.SubItemByItemAndNum(27, 1);
                BagManage.Instance.SubItemByItemAndNum(28, 1);
                BagManage.Instance.SubItemByItemAndNum(29, 1);
                BagManage.Instance.Add(31);
                SetStateFinish();
                callBack();
            });
        }
        else if (mCfg.type == (int)MISSIONTYPE.SHAKE)
        {
            for (int i = 0; i < gui.mSpineRoot.Length; i++)
            {
                StartShake(gui.mSpineRoot[i], () =>{
                    SetStateFinish();
                    callBack();
                });
            }
         
        }
        else
        {
            SetStateFinish();
            callBack();

        }


    }



    // Call this function to start the shake effect
    public void StartShake(RectTransform targetImage, Action action = null)
    {
        float shakeDuration = 0.5f; // Duration of the shake
        float shakeMagnitude = 10f; // Magnitude of the shake
        RectTransform rectTransform = targetImage.GetComponent<RectTransform>();
        Vector3 originalPos = rectTransform.anchoredPosition;
        rectTransform.DOShakeAnchorPos(shakeDuration, shakeMagnitude, 10, 90, false, true)
            .OnComplete(() => {
                rectTransform.anchoredPosition = originalPos;
                if (action != null)
                {
                    action.Invoke();
                }
                }) ;
    }
    private void ShowReward()
    {
        List<DropNode> itemIdList = DropManager.Instance.GetDropItemId2(MissionManage.GetCurrdDrop(1));
        string str = "";
        for (int i = 0; i < itemIdList.Count; i++)
        {
            if (itemIdList[i].itemId > 0)
            {
                itmeconfigData mCfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", itemIdList[i].itemId);

            }
            else if (itemIdList[i].itemId == -2)
            {
                GameManage.userData.SetAddGoldValue(itemIdList[i].count);
                if (str == "")
                {
                    str = "获得金币:" + itemIdList[i].count;
                }
                else
                {
                    str = str + "  获得金币:" + itemIdList[i].count;
                }


            }
            else if (itemIdList[i].itemId == -1)
            {
                GameManage.userData.SetAddExpValue(itemIdList[i].count);


                if (str == "")
                {
                    str = "获得经验:" + itemIdList[i].count;
                }
                else
                {
                    str = str + "     获得经验:" + itemIdList[i].count;
                }
            }
        }
        if (itemIdList.Count > 0)
        {
            Common.Instance.ShowTips(str);
        }
      
    }
    public Vector3 SetHangPos(string hangPosStr)
    {
        string[] strArr = hangPosStr.Split(",");
        float posX = float.Parse(strArr[0]);
        float posY = float.Parse(strArr[1]);
        return new Vector3(posX, posY, 0);
    }
}
