//敌人类
using System;
using UnityEngine;

public class Pet : BattleUnitBase
{
  
    public pet_lvconfigData m_cfg;
    public Action<Pet, GameObject> UpdataAttributeChange; // 状态改变时的事件
    private GameObject infoPanelGo;
    private DelayedCoroutineData hurtDelayed;
    PetNode petNode;
    // 在敌人类中可以添加敌人的特有属性和行为
    public Pet(int petId)
    {
        petNode = GameManage.GetPetById(petId);
        m_cfg = GetCfgManage.Instance.GetCfgByNameAndId<pet_lvconfigData>("pet_lv", petId * 10000 + petNode.level);
        hp = m_cfg.hp;
        attack = m_cfg.attack;
        name = petNode.name;
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
            
        }
      
    }
    public override void AddHp(int addNum)
    {
        hp += addNum;
        UpdataAttributeChange(this, infoPanelGo);
    }
}
