// ���ܹ�����
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
    // �¼����������ͷ�ʱ����
    public event Action<ISkill> OnSkillReleased;

    // �ͷż���
    public void ReleaseSkill(ISkill skill)
    {
        // ִ�м���Ч��
        skill.Execute();

        // ���������ͷ��¼�
        OnSkillReleased?.Invoke(skill);
    }
}