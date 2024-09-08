// 技能效果接口
using System.Collections.Generic;

public interface ISkillEffect
{
    void ApplyEffect(BattleUnitBase source, List<BattleUnitBase> target);
    cfgbase GetSkillLvCfg();
}