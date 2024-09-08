using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DateStringStorage
{
    private Dictionary<DateTime, string> dateToStringMap = new Dictionary<DateTime, string>();
    private string saveFilePath;

    public DateStringStorage(string saveFileName)
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        LoadData();
    }

    // �Ӵ洢�ļ��м�������
    private void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string[] lines = File.ReadAllLines(saveFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 4)
                {
                    DateTime date = DateTime.Parse(parts[0] + ":" + parts[1] + ":"+parts[2]);
                    string str = parts[3];
                    dateToStringMap[date] = str;
                }
            }
        }
    }

    // �����ݱ��浽�洢�ļ���
    private void SaveData()
    {
        List<string> lines = new List<string>();
        foreach (var pair in dateToStringMap)
        {
            lines.Add(pair.Key.ToString() + ":" + pair.Value);
        }
        File.WriteAllLines(saveFilePath, lines);
    }

    // �洢�ַ�����ָ������
    public void StoreString(DateTime date, string str)
    {
        dateToStringMap[date] = str;
        SaveData();
    }

    // �������ڻ�ȡ�洢���ַ���
    public string RetrieveString(DateTime date)
    {
        if (dateToStringMap.TryGetValue(date, out string str))
        {
            return str;
        }
        return null; // ���ָ������û�ж�Ӧ���ַ������򷵻� null
    }

    // �޸�ָ�����ڴ洢���ַ���
    public void ModifyString(DateTime date, string newStr)
    {
        if (dateToStringMap.ContainsKey(date))
        {
            dateToStringMap[date] = newStr;
            SaveData();
        }
        else
        {
            StoreString(date, newStr);
        }
    }
}
