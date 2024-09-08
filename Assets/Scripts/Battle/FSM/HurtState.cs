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
        // �ڽ����ͷż���״̬ʱ�Ĳ���
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
        // ���ͷż���״̬ʱ�Ĳ���
        // ���Կ��ǲ��ż��ܶ�����
    }

    public void Exit()
    {
 
    }
}
