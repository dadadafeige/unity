using System;
using UnityEngine;

[System.Serializable]
public class ItemNode
{
    public string name; // 物品名称
    public int id; // 物品ID
    [NonSerialized]
    public itmeconfigData mCfg;
    [NonSerialized]
    public Sprite icon;
    public int count = 0;
    public ItemNode(string str)
    {
        string[] strArr = str.Split(",");
        id = int.Parse(strArr[0]);
        mCfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", id);
        count += int.Parse(strArr[1]);
        if (count > 99)
        {
            count = 99;
        }

    }
    public ItemNode(int itemId)
    {
        mCfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", itemId);
        id = itemId;
        count += 1;
    }
    public ItemNode()
    {
        
    }
    public void addCount(int count)
    {
        this.count += count;
        if (this.count > 99)
        {
            this.count = 99;
        }
    }
    public void addCount(string str)
    {
        string[] strArr = str.Split(",");
        count += int.Parse(strArr[1]);
        if (count > 99)
        {
            count = 99;
        }
    }
}