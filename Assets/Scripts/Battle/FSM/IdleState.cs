// ����״̬�ࣨ���磬Idle״̬��
using System.Collections.Generic;
using Unity.VisualScripting;

public class IdleState : IState
{

    private DragonBonesController dragon;
    public IdleState(List<BattleUnitBase> targets, DragonBonesController dragon = null, BattleUnitBase self = null)
    {
        this.dragon = dragon;
    }
    public void Enter()
    {
        if (dragon != null)
        {
            dragon.PlayAnimation(dragon.armatureComponent.animation.animationNames[0], true);

        }
        // ����Idle״̬ʱ�Ĳ���
    }

    public void Update()
    {
        // ��Idle״̬ʱ�Ĳ���
    }

    public void Exit()
    {
        // �˳�Idle״̬ʱ�Ĳ���
    }
}