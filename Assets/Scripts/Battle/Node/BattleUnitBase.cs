using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitBase
{
    public int hp;
    public int mp = 2;
    public int attack;
    public int defense;
    public float magic_resistance;
    public string name;
    public StateMachine stateMachine;
    public RectTransform obj_root;
    // 储存行动方法
    public delegate void ActionPhaseCallback();
    // 储存行动方法
    private ActionPhaseCallback actionPhaseCallback;
    public DragonBonesController bones;
    public void SetActionPhaseCallback(ActionPhaseCallback callback)
    {
        actionPhaseCallback = callback;

    }
    public void BindBones(DragonBonesController bones)
    {
        this.bones = bones;

    }
    public void OnActionPhaseComplete()
    {
        // 执行回调
        actionPhaseCallback?.Invoke();
     
        
    }
    public virtual void SubHp(int subNum)
    {
       
    }
    public virtual void AddHp(int addNum)
    {
    
    }
}
