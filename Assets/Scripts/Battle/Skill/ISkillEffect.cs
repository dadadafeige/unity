// ����Ч���ӿ�
using System.Collections.Generic;

public interface ISkillEffect
{
    void ApplyEffect(BattleUnitBase source, List<BattleUnitBase> target);
    cfgbase GetSkillLvCfg();
}