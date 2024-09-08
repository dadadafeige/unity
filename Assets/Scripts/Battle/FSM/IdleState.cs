// 具体状态类（例如，Idle状态）
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
        // 进入Idle状态时的操作
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