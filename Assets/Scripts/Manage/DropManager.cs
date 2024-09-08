using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropNode
{
    public int id;
    public int itemId;
    public int count;
    public DropNode(int id,int itemId,int count)
    {
        this.count = count;
        this.id = id;
        this.itemId = itemId;
    }

}
public class DropManager
{
    private static DropManager instance;
    private Dictionary<int, int> dropOrderMap = new Dictionary<int, int>();
    public static DropManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DropManager();
            }
            return instance;
        }
    }
    public List<DropNode> GetDropItemId2(int cfgId)
    {
        List<DropNode> itemIdList = new List<DropNode>();
        if (cfgId == -1)
        {
            return itemIdList;
        }
        dropconfigData dropcfg = GetCfgManage.Instance.GetCfgByNameAndId<dropconfigData>("drop", cfgId);
        int itemId;
        string[] strings = dropcfg.output.Split(",");
    
        if (dropcfg.mode == 1)
        {
            if (!dropOrderMap.ContainsKey(cfgId))
            {
                dropOrderMap.Add(cfgId, 0);
            }
            else
            {
                dropOrderMap[cfgId]++;
            }
            if (dropOrderMap[cfgId] >= strings.Length)
            {
                dropOrderMap[cfgId] = 0;
            }
            itemId = int.Parse(strings[dropOrderMap[cfgId]]);

            DropNode drop = new DropNode(cfgId, itemId, 1);
            itemIdList.Add(drop);
        }
        else if (dropcfg.mode == 2)
        {
            string[] min_quantitys = dropcfg.min_quantity.Split(",");
            string[] max_quantitys = dropcfg.max_quantity.Split(",");
            System.Random random = new System.Random();
            for (int i = 0; i < strings.Length; i++)
            {
                try
                {
                    int min = int.Parse(min_quantitys[i]);
                    int max = int.Parse(max_quantitys[i]);
                    int randomNumber = random.Next(min, max + 1);
                    DropNode drop = new DropNode(cfgId, int.Parse(strings[i]), randomNumber);
                    itemIdList.Add(drop);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("An error occurred: " + e.Message);
                }
           
            }
        }
        else
        {
            itemId = int.Parse(strings[0]);
            DropNode drop = new DropNode(cfgId, itemId, 1);
            itemIdList.Add(drop);
        
        }
        return itemIdList;
    }
    public int GetDropItemId(int cfgId)
    {
        dropconfigData dropcfg = GetCfgManage.Instance.GetCfgByNameAndId<dropconfigData>("drop", cfgId);
        int itemId;
        string[] strings = dropcfg.output.Split(",");
        if (dropcfg.mode == 1)
        {
            if (!dropOrderMap.ContainsKey(cfgId))
            {
                dropOrderMap.Add(cfgId, 0);
            }
            else
            {
                dropOrderMap[cfgId]++;
            }
            if (dropOrderMap[cfgId] >= strings.Length)
            {
                dropOrderMap[cfgId] = 0;
            }
            itemId = int.Parse(strings[dropOrderMap[cfgId]]);
        }
        else
        {
            itemId = int.Parse(strings[0]);
        }
        return itemId;
    }
}
