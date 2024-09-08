using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class CastSkillState : IState
{
    private ISkillEffect skillEffect;
    private BattleUnitBase source;
    private List<BattleUnitBase> targets;
    DragonBonesController dragon;

    public CastSkillState(ISkillEffect effect, BattleUnitBase skillSource, List<BattleUnitBase> skillTarget, DragonBonesController dragon)
    {
        skillEffect = effect;
        source = skillSource;
        targets = skillTarget;
        this.dragon = dragon;
    }

    public void Enter()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            Transform arrow = targets[i].obj_root.Find("arrow");
            Transform aperture = targets[i].obj_root.Find("aperture");
            if (aperture != null)
            {
                aperture.gameObject.SetActive(true);
            }
            arrow.gameObject.SetActive(true);
        }
     

        // 在进入释放技能状态时的操作
        dragon.PlayAnimation("05_FuWenAttack", false, () =>
        {
            for (int i = 0; i < targets.Count; i++)
            {
                Transform arrow = targets[i].obj_root.Find("arrow");
                Transform aperture = targets[i].obj_root.Find("aperture");
                if (aperture != null)
                {
                    aperture.gameObject.SetActive(false);
                }

                arrow.gameObject.SetActive(false);
            }

     

            PlayertCharacterNode playert = source as PlayertCharacterNode;
            playert.CleanMp(skillEffect.GetSkillLvCfg() as skill_lvcnofigData);
            skillEffect.ApplyEffect(source, targets);
          
            source.stateMachine.SetState(new IdleState(new List<BattleUnitBase>() { source }));
        });
    }

    public void Update()
    {
        // 在释放技能状态时的操作
        // 可以考虑播放技能动画等
    }

    public void Exit()
    {
        // 在退出释放技能状态时的操作
        Debug.Log($"{source.name} 完成释放技能");

        // 应用技能效果


        // 切换回 Idle 状态
      
    }
}
