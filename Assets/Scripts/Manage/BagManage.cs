using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BagManage
{
    public int space = 40; // �����ռ�
    public List<ItemNode> items = new List<ItemNode>(); // ��Ʒ�б�

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    private static BagManage instance;

    public static BagManage Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BagManage();
            }
            return instance;
        }
    }
    public void SaveItems()
    {
        InventoryData data = new InventoryData();
        data.items = items;
        SaveSystem.SaveData(data, "InventoryData");
    }

    // �ӱ��ش洢����items�б�
    public void LoadItems()
    {
        InventoryData data = SaveSystem.LoadData<InventoryData>("InventoryData");
        if (data != null)
        {
            items = data.items;
           
        }
    }
    // �����Ʒ������
    public bool Add(string str)
    {
        if (items.Count < space)
        {
            string[] strArr = str.Split(",");
            ItemNode item = GetItemById(int.Parse(strArr[0]) );
            if (item == null)
            {
                item = new ItemNode(str);
                items.Add(item);
            }
            else
            {
                if (item.mCfg.limit_num >= item.count)
                {
                    return false;
                }
                item.addCount(int.Parse(strArr[1]));
            }

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
            SaveItems();
            return true;
        }
        else
        {
            Debug.Log("��������");
            return false;
        }
    }
    public bool Add(int itemId)
    {
        ItemNode item = GetItemById(itemId);
        if (item != null)
        {
            item.addCount(1);
            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
            SaveItems();
            return true;
        }
        if (items.Count < space)
        {
            if (item == null)
            {
                item = new ItemNode(itemId);
                items.Add(item);
            }
            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
            SaveItems();
            return true;
        }
        else
        {
            Debug.Log("��������");
            return false;
        }

    }
    public ItemNode GetItemById(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (id == items[i].id)
            {
                if (items[i].mCfg == null)
                {
                    items[i].mCfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", id);
                }
                return items[i];
            }
        }
        return null;
    }
    public bool SubItemByItemAndNum(ItemNode item,int num)
    {
        if (item.count < num)
        {
            Common.Instance.ShowTips("��Ʒ����");
            return false;
        }
        else
        {
            item.count -= num;
            if (item.count == 0)
            {
                RemoveOne(item);
            }
            SaveItems();
            return true;
        }
      

    }
    public bool SubItemByItemAndNum(int itemId, int num)
    {
        ItemNode item = BagManage.Instance.GetItemById(itemId);
        if (item.count < num)
        {
            //Common.Instance.ShowTips("��Ʒ����");
            return false;
        }
        else
        {
            item.count -= num;
            if (item.count == 0)
            {
                RemoveOne(item);
            }
            SaveItems();
            return true;
        }


    }
    // �Ƴ���Ʒ�ӱ���
    public void RemoveOne(ItemNode item)
    {
        item.count--;
        if (item.count <= 0)
        {
            items.Remove(item);
        }
       

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        SaveItems();
    }
}