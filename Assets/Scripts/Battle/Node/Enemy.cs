//敌人类
using System;
using UnityEngine;

public class Enemy: BattleUnitBase
{
  
    public enemy_attributecnofigData m_cfg;
    public Action<Enemy,GameObject> UpdataAttributeChange; // 状态改变时的事件
    private GameObject infoPanelGo;
    private DelayedCoroutineData hurtDelayed;
    // 在敌人类中可以添加敌人的特有属性和行为
    public Enemy(int enemyId)
    {
        m_cfg = GetCfgManage.Instance.GetCfgByNameAndId<enemy_attributecnofigData>("enemy_attribute", enemyId);
        hp = m_cfg.hp;
        attack = m_cfg.attack;
        defense = m_cfg.defense;
        name = m_cfg.name;
        magic_resistance = 0;
        stateMachine = new StateMachine();
        stateMachine.OnStateChange += HandleStateChange;
    }
    public void HandleStateChange()
    {


    }
    public void BindObj(GameObject go)
    {
        infoPanelGo = go;

    }
    private void Update()
    {
        stateMachine.Update();
    }
    public override void SubHp(int subNum)
    {
        hp -= subNum;
        hurtDelayed = DelayedActionProvider.Instance.DelayedAction(() =>
        {
            UpdataAttributeChange(this, infoPanelGo);
            stateMachine.SetState(new HurtState(this));
            hurtDelayed = null;
        },0.5f);
        if (hp <= 0)
        {
            BattleManager.Instance.addExp += m_cfg.exp;
        }
      
    }
    public override void AddHp(int addNum)
    {
        hp += addNum;
        UpdataAttributeChange(this, infoPanelGo);
    }
}
