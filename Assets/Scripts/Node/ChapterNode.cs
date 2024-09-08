using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChapterNode
{
    public string id { get; }
    public Dictionary<int, plotconfigData> chapterCfg { get; }
    public Dictionary<int, PlotNode> plotNodeMap { get; } = new Dictionary<int, PlotNode>();
    public PlotNode firstPlot = null;

    public ChapterNode(string id)
    {
        this.id = id;
        chapterCfg = GetCfgManage.Instance.GetCfgByName<plotconfigData>(id);
        initPlot();
    }

    public PlotNode GetPlotById(int id)
    {
        if (plotNodeMap.ContainsKey(id))
        {
            return plotNodeMap[id];
        }
        else
        {
            Debug.Log("找不到对应章节");
            return null;
        }
    }
    private void initPlot()
    {
        //PlotNode temporaryNode = null;
        //foreach (plotconfigData item in chapterCfg.Values)
        //{
        //    if (!plotNodeMap.ContainsKey(item.id) && item.link == "")
        //    {
        //        PlotNode plotNode = new PlotNode(item, this);
        //        if (firstPlot == null)
        //        {
        //            firstPlot = plotNode;
        //        }
            
        //        if (temporaryNode == null)
        //        {
        //            plotNode.SetNextPlot(plotNode);
        //        }
        //        else
        //        {
        //            temporaryNode.SetNextPlot(plotNode);

        //        }
        //        temporaryNode = plotNode;
        //        plotNodeMap.Add(item.id,plotNode);
        //    }
        //}
    }
    public plotconfigData GetPlotConfigById(int id)
    {
        Debug.Log(id);
        if (chapterCfg.ContainsKey(id))
        {
            return chapterCfg[id];
        }
        return null;

    }
}

