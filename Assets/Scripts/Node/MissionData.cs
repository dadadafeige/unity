using System.Collections.Generic;
using System;

[Serializable]
public class MissionData : IGameData
{
  
    public Dictionary<int, MissionNode> missionMap = new Dictionary<int, MissionNode>();
}
