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
    PURE_TALE = 1, //���Ի�
    DRAW_RUNE = 2,//����
    FLIPBRAND = 3,//ץ���
    GETREWARD = 4,//��ý���
    GOFISH = 5,//����
    BATTLE = 6,//ս��
    FEEDINGFISH = 7,//ι��
    PICKINGNMUSHROOMS = 8,//��Ģ��
    LOG= 9,//�ռ�
    PUZZLE = 10,//ƴͼ
    FLANKER = 11,//��ľ�Ա�
    COLLECTMATERIAL = 12,//�ռ�����
    GUESSINGPUZZLE = 13,//������ѵ��
    LONGTIME = 14,//�ܾúܾ��Ժ�
    HURRYWAY = 15,//��·
    BALANCEBALL = 16,//ƽ����
    RECALLWAY = 17,//����·��
    GUESSINGFISTS = 18,//��ȭ
    GETPET = 19,//��ó���
    NUMBERMEMORY = 20,//������  ��С����
    FINDSOMETHING = 21,//���ر�
    SEABEDPUZZLE = 22,//����
    COLLECTMATERIAL2 = 23,//ը����
    SHOOTINGSUN = 24,//����
    FINDDIFFEREN = 25,//�Ҳ�

    PUZZLE2 = 26,//ƴ����
    RECALLWAY2 = 27,//�Թ�
    RECOVERYHP = 28,//�ָ�Ѫ��
    FINDDIFFEREN2 = 29,//�Ҳ�2
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

    // �ӱ��ش洢����items�б�
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
