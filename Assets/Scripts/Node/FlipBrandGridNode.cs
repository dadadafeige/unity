using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UIFlipBrand;

public class FlipBrandGridNode
{
    // Start is called before the first frame update
    // 可以在这里添加格子的属性
    // 例如，格子的状态、坐标等等
    public flipbrandcnofigData cfg;
    public GameObject gridObj;
    public static event DataChangedEventHandler DataChanged;
    public bool isClick = false;
    public bool isReady = true;
    public FlipBrandRowNode root;
    public FlipBrandGridNode(int cfgId, FlipBrandRowNode root)
    {
        this.root = root;
        cfg = GetCfgManage.Instance.GetCfgByNameAndId<flipbrandcnofigData>("flipbrandgrid", cfgId);
        Debug.Log(cfg.icon);
    }
    public void UpdataGrid(int cfgType)
    {
        int cfgId;
        if (cfgType == 4)
        {
            Thread.Sleep(10);
            System.Random random = new System.Random();
            cfgId = random.Next(4, 5);
        }
        else
        {
            cfgId = cfgType;

        }
        cfg = GetCfgManage.Instance.GetCfgByNameAndId<flipbrandcnofigData>("flipbrandgrid", cfgId);
        DataChanged(this);
    }
  
    public void UpdataReady(bool isReady)
    {
        this.isReady = isReady;
        DataChanged(this);
    }
    public void UpdataClick(bool isClick)
    {
        this.isClick = isClick;
        DataChanged(this);
    }
    public void Refresh(int cfgType)
    {
        isClick = false;
        isReady = true;
        UpdataGrid(cfgType);

    }
    public void BindGrid(GameObject go)
    {
        gridObj = go;


    }


}
