using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;
using Unity.VisualScripting.FullSerializer;
//using Newtonsoft.Json;

// ¶ÁÈ¡ JSON ÎÄ¼þ


public class GetCfgManage
{
    private static GetCfgManage instance;
    private Dictionary<string, System.Object> cfgMap = new Dictionary<string, System.Object>();
    public static GetCfgManage Instance
    {
        get
        {
            if (instance == null)
            {

                instance = new GetCfgManage();
            }
            return instance;
        }
    }
     public T GetCfgByNameAndId<T>(string jsonFileName,int cfgId)
    {

        if (cfgMap.ContainsKey(jsonFileName))
        {
            Dictionary<int, T> dataMapEx = (Dictionary<int, T>)cfgMap[jsonFileName];
            return dataMapEx[cfgId];

        }
        string assetBundleName = "config/" + jsonFileName;
        string assetName = jsonFileName;
        TextAsset textAsset = AssetBundleManager.Instance.LoadAsset<TextAsset>(assetBundleName, assetName);
  
     //   StreamReader sr = new StreamReader(Application.dataPath + "/AssetBundle/Config/" + jsonFileName+".json");
        string json = textAsset.text;
            //sr.ReadToEnd();
        String[] strs = json.Split(",}");
        Dictionary<int, T> dataMap = new Dictionary<int, T>();
        T[] personArr = JsonConvert.DeserializeObject<T[]>(json);
        for (int i = 0; i < personArr.Length; i++)
        {
            T person = personArr[i];
            cfgbase data = person as cfgbase;
            Debug.Log(jsonFileName);
            dataMap.Add(data.id, person);
        }
        cfgMap.Add(jsonFileName, dataMap);
        return dataMap[cfgId];
        //  storycnofigDatas person = JsonConvert.DeserializeObject<storycnofigDatas>(json);

    }
    public Dictionary<int, T> GetCfgByName<T>(string jsonFileName)
    {

        if (cfgMap.ContainsKey(jsonFileName))
        {
            Dictionary<int, T> dataMapEx = (Dictionary<int, T>)cfgMap[jsonFileName];
            return dataMapEx;

        }
        string assetBundleName = "config/" + jsonFileName;
        string assetName = jsonFileName;
        TextAsset textAsset = AssetBundleManager.Instance.LoadAsset<TextAsset>(assetBundleName, assetName);
        //  StreamReader sr = new StreamReader(Application.dataPath + "/AssetBundle/Config/" + jsonFileName + ".json");
        string json = textAsset.text; 
            //sr.ReadToEnd();
        String[] strs = json.Split(",}");
        Dictionary<int, T> dataMap = new Dictionary<int, T>();
        T[] personArr = JsonConvert.DeserializeObject<T[]>(json);
        for (int i = 0; i < personArr.Length; i++)
        {
            T person = personArr[i];
            cfgbase data = person as cfgbase;

            dataMap.Add(data.id, person);
        }
        cfgMap.Add(jsonFileName, dataMap);
        return dataMap;
        //  storycnofigDatas person = JsonConvert.DeserializeObject<storycnofigDatas>(json);

    }
}


 
