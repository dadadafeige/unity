using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public enum MISSIONSTATE
{
    RECEIVE,
    UNFINISH,
    FINISH,
    DAILY,

}
public enum MISSIONTYPE
{
    PURE_TALE = 1, //纯对话
    DRAW_RUNE = 2,//画符
    FLIPBRAND = 3,//抓蚯蚓
    GETREWARD = 4,//获得奖励
    GOFISH = 5,//钓鱼
    BATTLE = 6,//战斗
    FEEDINGFISH = 7,//喂鱼
    PICKINGNMUSHROOMS = 8,//采蘑菇
    LOG= 9,//日记
    PUZZLE = 10,//拼图
    FLANKER = 11,//草木皆兵
    COLLECTMATERIAL = 12,//收集材料
    GUESSINGPUZZLE = 13,//记忆力训练
    LONGTIME = 14,//很久很久以后
    HURRYWAY = 15,//赶路
    BALANCEBALL = 16,//平衡球
    RECALLWAY = 17,//记忆路线
    GUESSINGFISTS = 18,//猜拳
    GETPET = 19,//获得宠物
    NUMBERMEMORY = 20,//点数字  从小到大
    FINDSOMETHING = 21,//找秘宝
    SEABEDPUZZLE = 22,//龙珠
    COLLECTMATERIAL2 = 23,//炸弹人
    SHOOTINGSUN = 24,//射日
    FINDDIFFEREN = 25,//找茬

    PUZZLE2 = 26,//拼镜子
    RECALLWAY2 = 27,//迷宫
    RECOVERYHP = 28,//恢复血量
    FINDDIFFEREN2 = 29,//找茬2
    FEEDINGFISH2 = 30,
    PUZZLE3 = 31,
    SHAKE = 32,
}
public static class MissionManage
{
    public enum SCENENUM {
        HOME,
        FORGE,
      
    };

    static MissionNode curMission;
    static Dictionary<int, MissionNode> missionMap  = new Dictionary<int, MissionNode>();
    public static MissionNode GetMissionNodeById(int id)
    {
        if (id == 0)
        {
            return null;
        }
        if (missionMap.ContainsKey(id))
        {
            return missionMap[id];

        }
        missioncnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<missioncnofigData>("mission", id);
        MissionNode missionNode = new MissionNode(cfg);
        missionMap.Add(id, missionNode);
        SaveItems();
        return missionNode;

    }
    public static void SaveItems()
    {
        MissionData data = new MissionData();
        data.missionMap = missionMap;
        SaveSystem.SaveData(data, "MissionData");
    }

    // 从本地存储加载items列表
    public static void LoadItems()
    {
        MissionData data = SaveSystem.LoadData<MissionData>("MissionData");
        if (data != null)
        {
            missionMap = data.missionMap;

        }
    }
    public static int ShowDescription(Action callback = null)
    {
        MissionNode mission = GetMissionNodeById(GameManage.curGameMissionId);
        if (mission == null)
        {
            if (callback != null)
            {
                callback.Invoke();
            
            }
            return -1;

        }
        if (mission.mCfg.game_id == -1)
        {
            return -1;
        }
        game_listcnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<game_listcnofigData>("game_list", mission.mCfg.game_id);
        int showId = -1;
        if (mission.mCfg.game_id == 32)
        {
            showId = 32;
        }
        else if (mission.mCfg.game_id == 28)
        {
            showId = 33;
        }
        else
        {
            showId = cfg.game_type;
        }
        UIGameDescription uIGame = Common.Instance.ShowGameDescription(showId);
        if (callback != null)
        {
            uIGame.SetDestroyCallback(callback);
        }
        
        return -1;
    }

    public static int GetCurrdDrop(int diff)
    {
        MissionNode mission = GetMissionNodeById(GameManage.curMissionId);
        if ( mission.mCfg.game_id == -1)
        {
            return -1;
        }
        game_listcnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<game_listcnofigData>("game_list", mission.mCfg.game_id);
        if (cfg.dropId == "")
        {
            return -1;
        }
        string[] strings = cfg.dropId.Split(",");
        if (strings.Length > diff - 1 && strings[diff - 1] != null)
        {
            return int.Parse(strings[diff - 1]);
        }
        return -1;
    }

}
