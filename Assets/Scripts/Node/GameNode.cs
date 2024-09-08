using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
[Serializable]
public class GameNode
{
   
   public game_listcnofigData mCfg;
   public GameNode(int cfgId)
    {
        mCfg = GetCfgManage.Instance.GetCfgByNameAndId<game_listcnofigData>("game_list", cfgId);


    }
}
