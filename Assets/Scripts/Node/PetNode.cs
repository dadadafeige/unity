using System;
using UnityEngine;
using static UserData;

[System.Serializable]
public class PetNode
{
    public int id;
    public string name;
    public int level;
    public delegate void UpdeaPetLvExp(PetNode pet);
    [NonSerialized]
    public UpdeaPetLvExp updeaPetLvExp;
    public petconfigData petconfig {
        get
        {
            return  GetCfgManage.Instance.GetCfgByNameAndId<petconfigData>("pet", id);
        }
    }
    public pet_lvconfigData petLvCfg
    {
        get
        {
          
            return GetCfgManage.Instance.GetCfgByNameAndId<pet_lvconfigData>("pet_lv", id * 10000 + level);
        }
    }
    public PetNode(int id, int level = 1)
    {
        this.level = level;
        this.id = id;
        //petconfig = GetCfgManage.Instance.GetCfgByNameAndId<petconfigData>("pet", id);
        //petLvCfg =  GetCfgManage.Instance.GetCfgByNameAndId<pet_lvconfigData>("pet_lv", id * 10000 + level);

    }
    public void UpLv(){
        level++;
        updeaPetLvExp?.Invoke(this);
        // petLvCfg = GetCfgManage.Instance.GetCfgByNameAndId<pet_lvconfigData>("pet_lv", id * 10000 + level);
        GameManage.SavePets();

    }

    // 其他宠物属性和方法可以根据需要添加
}