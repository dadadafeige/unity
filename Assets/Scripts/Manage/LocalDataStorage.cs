using System.Collections.Generic;
using UnityEngine;

public enum SAVEENUM
{



};
public static class LocalDataStorage
{
    // ��������
    public static void SaveData(SAVEENUM key, string value)
    {
        PlayerPrefs.SetString(key.ToString(), value);
        PlayerPrefs.Save(); // ���浽����
    }

    // ��������
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
        PlayerPrefs.Save(); // ���浽����
    }

    // ����List<Item>����
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
    // ɾ������
    public static void DeleteData(SAVEENUM  key)
    {
        PlayerPrefs.DeleteKey(key.ToString());
        PlayerPrefs.Save(); // ���浽����
    }

    // ʾ���÷�
    //void Start()
    //{
    //    // ��������
    //    SaveData("PlayerName", "John Doe");

    //    // ��������
    //    string playerName = LoadData("PlayerName");
    //    Debug.Log($"Player Name: {playerName}");

    //    // ɾ������
    //    DeleteData("PlayerName");
    //}
}
