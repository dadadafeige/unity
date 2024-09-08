using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class Common
{
    private static Common instance;
    public Dictionary<int, string> big_digit_word_map = new Dictionary<int, string>() 
    { [1] = "<sprite=8>", [2] = "<sprite=7>", [3] = "<sprite=6>", [4] = "<sprite=9>", [5] = "<sprite=5>",
      [6] = "<sprite=2>", [7] = "<sprite=3>", [8] = "<sprite=4>", [9] = "<sprite=0>", [10] = "<sprite=1>" };
   
    public static Common Instance
    {
        get
        {
            if (instance == null)
            {

                instance = new Common();
            }
            return instance;
        }
    }
    // 将整数拆分为数字数组
    public string SplitNumber(int number)
    {
     
        StringBuilder str = new StringBuilder();

        List<int> ints = new List<int>();
        // 处理负数的情况
        if (number < 0)
        {
            return "";

        }
        if (number==0)
        {
            return "<sprite=0>";
        }
        // 将数字按位拆分，并逆序存储在列表中
        while (number > 0)
        {
            int digit = number % 10;
           
            ints.Add(digit);
            number /= 10;
        }

        // 如果数字为0，添加0到列表

       
        ints.Reverse();
        for (int i = 0; i < ints.Count; i++)
        {
            str.Append("<sprite=" + ints[i] + ">");

        }
        return str.ToString();
    }
    public void ShowGetReward(Transform tran, Action action = null)
    {
        UIGetReward gui = UiManager.OpenUI<UIGetReward>("UIGetReward");
        gui.SetData(tran, action);
    }
    public void ShowGetReward(Transform tran, int itemId, Action action = null)
    {
        UIGetReward gui = UiManager.OpenUI<UIGetReward>("UIGetReward");
        gui.SetData(tran, itemId, action);
    }
    public UIFeedingFishSettle ShowSettleUI(int resultType, int dropId, Action callBack, Action exitCallBack = null, Action nextCallBack = null)
    {
        UIFeedingFishSettle gui = UiManager.OpenUI<UIFeedingFishSettle>("UIFeedingFishSettle");
        MissionNode mission = MissionManage.GetMissionNodeById(GameManage.curGameMissionId);
        if (mission == null)
        {
            dropId = -1;

        }
        else
        {
            if (mission.mCfg.game_id == -1)
            {
                dropId = -1;
            }
            else
            {
                game_listcnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<game_listcnofigData>("game_list", mission.mCfg.game_id);
                if (cfg.chapter_limit <= GameManage.userData.unlockChapter)
                {
                    if (mission != null)
                    {
                        if (cfg.reward_num > mission.rewardNum)
                        {

                            if (resultType == 2 || resultType == 3)
                            {
                                mission.rewardNum++;
                            }
                        }
                        else
                        {
                            dropId = -1;


                        }
                    }

                }
            }
        }
        
       
            
         gui.SetCallBack(resultType, dropId, callBack, exitCallBack, nextCallBack);
        return gui;

    }
    
    public void ShowGetReward(Transform tran, Sprite sprite, Action action = null)
    {
        UIGetReward gui = UiManager.OpenUI<UIGetReward>("UIGetReward");
        gui.SetData(tran, sprite, action);
        
    }
    public void ShowTips(string str)
    {
        UITips gui = UiManager.OpenUI<UITips>("UITips");
        gui.SetLabel(str);
    }
    public  void OpenExitDialog(string str, Action callBack, Action callCancelBack = null)
    {
        UIExitDialog gui = UiManager.OpenUI<UIExitDialog>("UIExitDialog");
        if (callCancelBack == null)
        {

            gui.ShowUi(str, callBack);
        }
        else
        {
            gui.ShowUi(str, callBack, callCancelBack);
        }
    }
    public UIBag ShowBag()
    {
        UIBag iBag = UiManager.OpenUI<UIBag>("UIBag");
        return iBag;
    }
    public UIGameDescription ShowGameDescription(int id)
    {
        UIGameDescription uIGame = UiManager.OpenUI<UIGameDescription>("UIGameDescription");
        uIGame.SetDecById(id);
        return uIGame;
    }
    public UIShowBones ShowBones(string bonesName, Action callBack, string animName = null)
    {
        UIShowBones uIShow = UiManager.OpenUI<UIShowBones>("UIShowBones");
        uIShow.PlayBones(bonesName, () => {
            callBack.Invoke();
        }, animName);
        return uIShow;
    }

}
