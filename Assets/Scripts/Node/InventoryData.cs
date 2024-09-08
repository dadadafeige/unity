using System.Collections.Generic;
using System;

[Serializable]
public class InventoryData : IGameData
{
    public List<ItemNode> items = new List<ItemNode>();
}
