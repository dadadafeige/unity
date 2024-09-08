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
        // 在进入释放技能状态时的操作
        dragon.PlayAnimation("04_Death", false, () =>
        {
            dragon.gameObject.SetActive(false);


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
