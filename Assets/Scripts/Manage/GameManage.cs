using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;


public static class GameManage
{
    public static int curStoryId { get; set; } = 1;
    public static int curChapter = 1;
    public static int curMissionId { get; set; } = 1;
    public static UserData userData { get; set; } = new UserData();
    public static List<PetNode> petBag { get; set; } = new List<PetNode>();
    public static List<GameNode> gameList { get; set; } = new List<GameNode>();
    public static bool isNewBie = false;
    public static bool isMap = false;
    public static int curGameMissionId;
    public static string phone;
    public static List<CaseInfo> caseInfos;
    public static string version = "V1.0";
    public static string GetLanguage(int languageId)
    {
        languageData languageCfg = GetCfgManage.Instance.GetCfgByNameAndId<languageData>("language", languageId);
        return languageCfg.cn;
    }
   
    public static void InItPet()
    {
        LoadPets();
     

    }

    public static void InItGemeList()
    {
        


    }
    public static bool IsToday(DateTime dateTime)
    {
        DateTime today = DateTime.Today;
        return dateTime.Date == today;
    }
    public static void SaveUserData()
    {

        SaveSystem.SaveData(userData, "UserData");
    }

    // 从本地存储加载items列表
    public static void LoadUserData()
    {
        UserData data = SaveSystem.LoadData<UserData>("UserData");
        if (data != null)
        {
            userData = data;

        }
    }
    // 从本地存储加载items列表
    public static void LoadGame()
    {
        GameData data = SaveSystem.LoadData<GameData>("GameData");
        if (data != null)
        {
            DateTime today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            DateTime dateTime = new DateTime(data.Year, data.Month, data.Day);

            if (dateTime == today && data.gameList.Count >= 4)
            {
                gameList = data.gameList;
            }
            else
            {
                SaveGames();
            }
        }
        else
        {
            SaveGames();
        }
    }
    public static void SaveGames()
    {
        GameData data = new GameData(DateTime.Today);
        Dictionary<int, game_listcnofigData> cfgMap = GetCfgManage.Instance.GetCfgByName<game_listcnofigData>("game_list");
        List<int> ints = new List<int>();
        foreach (KeyValuePair<int, game_listcnofigData> item in cfgMap)
        {
            if (item.Value.day_dropId !=null && item.Value.day_dropId != "")
            {
                if (item.Value.mission_id <= GameManage.userData.unlockMissionId)
                {
                    ints.Add(item.Value.id);
                }
        
            }
        }
        gameList.Clear();
        if (ints.Count > 4)
        {
            if (ints.Count >= 10)
            {
                List<int> randomSelection = GetRandomSelection(ints, 4);

                for (int i = 0; i < randomSelection.Count; i++)
                {
                    gameList.Add(new GameNode(randomSelection[i]));
                }
            }
            else
            {
                List<int> randomSelection = GetRandomSelection(ints, 4);

                for (int i = 0; i < randomSelection.Count; i++)
                {
                    gameList.Add(new GameNode(randomSelection[i]));
                }
            }
          

        }
        else
        {
            for (int i = 0; i < ints.Count; i++)
            {
                gameList.Add(new GameNode(ints[i]));
            }
        }
        
        data.gameList = gameList;
        SaveSystem.SaveData(data, "GameData");
    }
    public static List<int> GetRandomSelection(List<int> list, int count,bool isBytype = false)
    {
        if (list == null || count < 0 || count > list.Count)
        {
            throw new ArgumentException("Invalid list or count");
        }

        List<int> listCopy = new List<int>(list);
        List<int> result = new List<int>();
        System.Random random = new System.Random();
        if (isBytype)
        {
            List<int> typeList = new List<int>();
            for (;;)
            {
                int index = random.Next(listCopy.Count);
                game_listcnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<game_listcnofigData>("game_list", listCopy[index]);
                if (!typeList.Contains(cfg.game_type))
                {
                    result.Add(listCopy[index]);
                    listCopy.RemoveAt(index); // 移除已选中的元素，确保不重复
                    typeList.Add(cfg.game_type);
                    if (result.Count == count)
                    {
                        break;
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(listCopy.Count);
                result.Add(listCopy[index]);
                listCopy.RemoveAt(index); // 移除已选中的元素，确保不重复
            }

        }
     

        return result;
    }
    public static void SavePets()
    {
        PetData data = new PetData();
        data.petBag = petBag;
        SaveSystem.SaveData(data, "PetData");
    }

    // 从本地存储加载items列表
    public static void LoadPets()
    {
        PetData data = SaveSystem.LoadData<PetData>("PetData");
        if (data != null)
        {
            petBag = data.petBag;

        }
        else
        {
            petBag.Add(new PetNode(1));
            petBag.Add(new PetNode(2));
        }
    }
    public static PetNode GetPetById(int id)
    {
        for (int i = 0; i < petBag.Count; i++)
        {
            if (id == petBag[i].id)
            {
                return petBag[i];
            }
        }
        return null;

    }
    /// <summary>
    /// 三阶贝塞尔曲线
    /// </summary>
    /// <param name="p0">起点</param>
    /// <param name="p1">控制点1</param>
    /// <param name="p2">控制点2</param>
    /// <param name="p3">终点</param>
    /// <param name="t">[0,1]</param>
    /// <returns></returns>
    public static Vector3 Bezier3(float t,Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 p2p3 = (1 - t) * p2 + t * p3;
        Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
        Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;
        return (1 - t) * p0p1p2 + t * p1p2p3;
    }

    //上面的代码虽然浅显易懂，但是计算挺复杂，研究了Cinemachine中的代码，发现人家的代码效率很高
    public static Vector3 Bezier3Ex(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        t = Mathf.Clamp01(t);
        float d = 1f - t;
        return d * d * d * p0 + 3f * d * d * t * p1
            + 3f * d * t * t * p2 + t * t * t * p3;
    }
    //贝塞尔切线算法
    public static Vector3 BezierTangent3(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        t = Mathf.Clamp01(t);
        return (-3f * p0 + 9f * p1 - 9f * p2 + 3f * p3) * (t * t)
            + (6f * p0 - 12f * p1 + 6f * p2) * t
            - 3f * p0 + 3f * p1;
    }
    public static void InIt()
    {
        InItPet();
    }
}
