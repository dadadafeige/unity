// CombatCharacterNode 类继承 MonoBehaviour
using System;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayertCharacterNode: BattleUnitBase
{
    public player_attributecnofigData m_cfg;

    // 其他 CombatCharacterNode 的属性和方法...
    public Action UpdataAttributeChange; // 状态改变时的事件

    private DelayedCoroutineData hurtDelayed;
    private int mpRate;
    public PlayertCharacterNode()
    {
        stateMachine = new StateMachine();
        stateMachine.OnStateChange += HandleStateChange;
        stateMachine.SetState(new IdleState(new List<BattleUnitBase>() { this }));
        InitAttribut();
    }
    public void InitAttribut()
    {
        AssetBundleManager.Instance.Initialize();
        int level = GameManage.userData.level;
        m_cfg = GetCfgManage.Instance.GetCfgByNameAndId<player_attributecnofigData>("player_attribute", level);
        hp = m_cfg.hp;
        hp = m_cfg.hp;
        attack = m_cfg.attack;
        defense = m_cfg.defense;
        name = "玩家";
        magic_resistance = m_cfg.magic_resistance;
    }
    private void Update()
    {
        stateMachine.Update();
    }
    public override void SubHp(int subNum)
    {
    

        hurtDelayed = DelayedActionProvider.Instance.DelayedAction(() =>
        {
            hp -= subNum;
            UpdataAttributeChange();
            stateMachine.SetState(new HurtState(this));
            hurtDelayed = null;
        }, 1f);
   
    }
    public  void CleanMp(skill_lvcnofigData skillcnofig)
    {
        mp = 0;
        mpRate = 2 / skillcnofig.cd;
        UpdataAttributeChange();
    }
    public void RestoreMp() {
        if (mp < 2)
        {
            mp += mpRate;
        }
        UpdataAttributeChange();
    }


    public override void AddHp(int addNum)
    {
        hp += addNum;
        UpdataAttributeChange();
    }
    private void HandleStateChange()
    {
        // 处理状态改变时的逻辑
    }

}
