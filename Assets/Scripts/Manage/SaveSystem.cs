using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;

public static class SaveSystem
{
    private static string GetSavePath<T>(string key) where T : IGameData
    {
        return Application.persistentDataPath + "/" + key + ".save";
    }

    public static void SaveData<T>(T data, string key) where T : IGameData
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetSavePath<T>(key);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
        Debug.Log("Data saved to " + path);
    }

    public static T LoadData<T>(string key) where T : IGameData
    {
        string path = GetSavePath<T>(key);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                T data = (T)formatter.Deserialize(stream);
                Debug.Log("Data loaded from " + path);
                return data;
            }
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return default(T);
        }
    }
    public static void DeleteAllSaveFiles()
    {
        string path = Application.persistentDataPath;
        if (Directory.Exists(path))
        {
            // 获取所有 .save 文件
            string[] saveFiles = Directory.GetFiles(path, "*.save");

            // 遍历并删除每个 .save 文件
            foreach (string file in saveFiles)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Deleted: {file}");
                }
                catch (IOException ioEx)
                {
                    Debug.LogError($"IOException: {ioEx.Message}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Exception: {ex.Message}");
                }
            }
        }
        else
        {
            Debug.LogError("Directory does not exist: " + path);
        }
    }
    public static void DeleteData<T>(string key) where T : IGameData
    {
        string path = GetSavePath<T>(key);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Data deleted from " + path);
        }
        else
        {
            Debug.LogWarning("No save file to delete at " + path);
        }
    }
}
