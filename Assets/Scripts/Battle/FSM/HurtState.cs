using System.Collections.Generic;
using UnityEngine;

public class HurtState : IState
{
   
    private BattleUnitBase source;
 
    DragonBonesController dragon;

    public HurtState(BattleUnitBase Source)
    {
        source = Source;
        this.dragon = Source.bones;
    }

    public void Enter()
    {
        // 在进入释放技能状态时的操作
        dragon.PlayAnimation("03_Hurt", false, () =>
        {
            if (source.hp <=0 )
            {
                source.stateMachine.SetState(new DieState(source));
            }
            else
            {
                source.stateMachine.SetState(new IdleState(new List<BattleUnitBase>() { source }, dragon));
            }
          
            
        });

    }

    public void Update()
    {
        // 在释放技能状态时的操作
        // 可以考虑播放技能动画等
    }

    public void Exit()
    {
 
    }
}
