// 技能管理器
using System;

public class SkillManager
{
    private static SkillManager m_Instance;
    public static SkillManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new SkillManager();
                return m_Instance;
            }
            else
            {
                return m_Instance;
            }

        }

    }

    public ISkillEffect GetSkillEffect()
    {
        skillcnofigData skillcnofig = GetCfgManage.Instance.GetCfgByNameAndId<skillcnofigData>("skill", 3);
        DamageEffect skill = new DamageEffect(skillcnofig);

        return skill;
    }
    // 事件：当技能释放时触发
    public event Action<ISkill> OnSkillReleased;

    // 释放技能
    public void ReleaseSkill(ISkill skill)
    {
        // 执行技能效果
        skill.Execute();

        // 触发技能释放事件
        OnSkillReleased?.Invoke(skill);
    }
}