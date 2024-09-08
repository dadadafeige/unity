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
    // �����ж�����
    public delegate void ActionPhaseCallback();
    // �����ж�����
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
        // ִ�лص�
        actionPhaseCallback?.Invoke();
     
        
    }
    public virtual void SubHp(int subNum)
    {
       
    }
    public virtual void AddHp(int addNum)
    {
    
    }
}
