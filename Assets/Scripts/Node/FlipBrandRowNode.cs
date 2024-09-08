using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class FlipBrandRowNode
{
    public List<FlipBrandGridNode> GridNodes { get; private set; }
    public int columnValue;
    public GameObject go;

    public FlipBrandRowNode(int columnValue,GameObject go)
    {
        GridNodes = new List<FlipBrandGridNode>();
        this.go = go;
        UpdataRowNode(columnValue);
    }
    public void UpdataRowNode(int columnValue)
    {
        this.columnValue = columnValue;
        // 初始化每行的格子
        for (int i = 0; i < columnValue; i++)
        {
            if (GridNodes.Count - 1 < i)
            {
                FlipBrandGridNode gridNode = new FlipBrandGridNode(1,this);
                GridNodes.Add(gridNode);
            }
            else
            {
                GridNodes[i].Refresh(1);

            }
        }


    }

    public void UpdateGridByType(int num ,int cfgType)
    {
        if (GridNodes.Count >  num)
        {
            GridNodes[num].UpdataGrid(cfgType);
        }
        

    }
}
