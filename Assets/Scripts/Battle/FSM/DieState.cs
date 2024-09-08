using System.Collections.Generic;
using UnityEngine;

public class DieState : IState
{
   
    private BattleUnitBase source;
 
    DragonBonesController dragon;

    public DieState(BattleUnitBase Source)
    {
        source = Source;
        this.dragon = Source.bones;
    }

    public void Enter()
    {
        // �ڽ����ͷż���״̬ʱ�Ĳ���
        dragon.PlayAnimation("04_Death", false, () =>
        {
            dragon.gameObject.SetActive(false);


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
