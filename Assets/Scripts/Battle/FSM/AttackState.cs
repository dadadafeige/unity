// 具体状态类（例如，Idle状态）
using DG.Tweening;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class AttackState : IState
{
    List<BattleUnitBase> targets;
    BattleUnitBase self = null;
    private float pro;
    private DragonBonesController dragon;
    public AttackState(List<BattleUnitBase> targets, DragonBonesController dragon, BattleUnitBase self = null,float pro = 1)
    {
        this.targets = targets;
        this.self = self;
        this.pro = pro;
        this.dragon = dragon;
    }
    public void Enter()
    {
        if (self.hp <= 0)
        {
          //  self.stateMachine.SetState(new IdleState(new List<BattleUnitBase>() { self, }, dragon));
            BattleManager.Instance.SwitchUnit();
            return;
        }
        
      
        dragon.PlayAnimation("05_Move", true);
        Transform tran = targets[0].obj_root.Find("hurt_point");
        Tween tween = dragon.transform.DOMove(tran.position,0.5f);
        Transform arrow = targets[0].obj_root.Find("arrow");
        Transform aperture = targets[0].obj_root.Find("aperture");
        TextMeshProUGUI damage = targets[0].obj_root.Find("damage").GetComponent<TextMeshProUGUI>();
        Vector3 initPos = damage.transform.localPosition;
        if (aperture != null)
        {
            aperture.gameObject.SetActive(true);
        }
    
        arrow.gameObject.SetActive(true);
        tween.onComplete = () =>
        {
        
            for (int i = 0; i < targets.Count; i++)
            {
                float subHpNum = GetSubHpNum(targets[i], self);
                subHpNum = subHpNum * pro;
                targets[i].SubHp((int)subHpNum);
                damage.text = "-" + (int)subHpNum;
                DelayedActionProvider.Instance.DelayedAction(() =>
                {
                    damage.gameObject.SetActive(true);
                    damage.transform.DOLocalMoveY(initPos.y + 50, 2).onComplete = () =>
                    {
                        damage.gameObject.SetActive(false);
                        damage.transform.localPosition = initPos;
                    };

                }, 0.5f);
               
                UnityEngine.Debug.Log($"造成 {(int)subHpNum} 点伤害给 {targets[i].name} 剩余血量 {targets[i].hp}");
            }
            dragon.PlayAnimation("02_Attack", false, () =>
            {
                if (aperture != null)
                {
                    aperture.gameObject.SetActive(false);
                }
                arrow.gameObject.SetActive(false);
                dragon.transform.localPosition = Vector3.zero;
                self.stateMachine.SetState(new IdleState(new List<BattleUnitBase>() { self }, dragon));
                BattleManager.Instance.SwitchUnit();
            });
        };
     

    }
    private int GetSubHpNum(BattleUnitBase targets, BattleUnitBase self)
    {
        int param1 = self.attack * targets.defense;
        int param2 = BattleManager.Instance.attackCoeff + targets.defense;
        int subHpNum = self.attack - (param1 / param2);
        return subHpNum;
    }
    public void Update()
    {
        // 在Idle状态时的操作
    }

    public void Exit()
    {
        // 退出Idle状态时的操作
    }
}