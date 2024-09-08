using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlotNode 
{
    public int id { get; }
    public plotconfigData plotCfg { get; }
    public MissionNode missionNode { get; }

    public PlotNode nextPlotNode;
    public bool isHandle = false;
    public PlotNode(plotconfigData plotCfg, MissionNode missionNode)
    {
        this.plotCfg = plotCfg;
        this.missionNode = missionNode;


    }
    public void SetNextPlot(PlotNode plotNode)
    {
        nextPlotNode = plotNode;

    }
}
