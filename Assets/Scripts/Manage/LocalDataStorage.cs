using System.Collections.Generic;
using UnityEngine;

public enum SAVEENUM
{



};
public static class LocalDataStorage
{
    // 保存数据
    public static void SaveData(SAVEENUM key, string value)
    {
        PlayerPrefs.SetString(key.ToString(), value);
        PlayerPrefs.Save(); // 保存到磁盘
    }

    // 加载数据
    public static string LoadData(SAVEENUM en)
    {
        string key = en.ToString();
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetString(key);
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found.");
            return null;
        }
    }
    public static void SaveItemList<T>(SAVEENUM en, List<T> itemList)
    {
        string key = en.ToString();
        string json = JsonUtility.ToJson(itemList);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save(); // 保存到磁盘
    }

    // 加载List<Item>数据
    public static List<T> LoadItemList<T>(SAVEENUM en)
    {
        string key = en.ToString();
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<List<T>>(json);
        }
        else
        {
            Debug.LogWarning("ItemList not found.");
            return new List<T>();
        }
    }
    // 删除数据
    public static void DeleteData(SAVEENUM  key)
    {
        PlayerPrefs.DeleteKey(key.ToString());
        PlayerPrefs.Save(); // 保存到磁盘
    }

    // 示例用法
    //void Start()
    //{
    //    // 保存数据
    //    SaveData("PlayerName", "John Doe");

    //    // 加载数据
    //    string playerName = LoadData("PlayerName");
    //    Debug.Log($"Player Name: {playerName}");

    //    // 删除数据
    //    DeleteData("PlayerName");
    //}
}
