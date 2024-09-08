using System.Collections.Generic;
using System;

[Serializable]
public class GameData : IGameData
{
    public List<GameNode> gameList { get; set; } = new List<GameNode>();
    public int Year;
    public int Month;
    public int Day;
    [NonSerialized]
    public DateTime dateTime;
 
    public GameData(DateTime dateTime)
    {
        this.dateTime = dateTime;
        Year = dateTime.Year;
        Month = dateTime.Month;
        Day = dateTime.Day;
    }
}
