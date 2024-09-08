using System.Collections.Generic;
using System;

[Serializable]
public class PetData : IGameData
{
    public List<PetNode> petBag = new List<PetNode>();
}
