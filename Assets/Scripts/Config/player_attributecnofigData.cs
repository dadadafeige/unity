using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class player_attributecnofigData : cfgbase
{
    public int exp;//经验
    public int fight;//战斗力
    public int defense;//   防御力
    public int attack;//攻击力
    public float magic_resistance;//魔抗
    public int hp;//生命值
    public string attack_sound = "player_attack";
    public string hurt_sound = "player_hurt";
    public string title;
    public string title_explain;



}
