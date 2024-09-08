using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageEffect : ISkillEffect
{
    skillcnofigData m_cfg;
    public DamageEffect(skillcnofigData skillcnofig)
    {
        m_cfg = skillcnofig;
    }

    public void ApplyEffect(BattleUnitBase self, List<BattleUnitBase> targets)
    {
        DragonBonesController bones = BattleManager.Instance.uIBattle.LoadBeatenEffect(m_cfg.beaten_effect);
        bones.PlayAnimation("Attack", false, () =>
        {
            BattleManager.Instance.SwitchUnit();
        });

        skill_lvcnofigData lv_cfg = GetCfgManage.Instance.GetCfgByNameAndId<skill_lvcnofigData>("skill_lv", m_cfg.id * 10000 + 1);
        for (int i = 0; i < targets.Count; i++)
        {
      
            TextMeshProUGUI damage = targets[i].obj_root.Find("damage").GetComponent<TextMeshProUGUI>();
            Vector3 initPos = damage.transform.localPosition;
            
            damage.text = "-" + lv_cfg.attack;
            DelayedActionProvider.Instance.DelayedAction(() =>
            {
                damage.gameObject.SetActive(true);
                damage.transform.DOLocalMoveY(initPos.y + 50, 2).onComplete = () =>
                {
                    damage.gameObject.SetActive(false);
                    damage.transform.localPosition = initPos;
                };

            }, 0.5f);

            targets[i].SubHp(lv_cfg.attack);
            UnityEngine.Debug.Log($"造成 {lv_cfg.attack} 点伤害给 {targets[i].name} 剩余血量 {targets[i].hp}");
        }
    }
    public cfgbase GetSkillLvCfg()
    {
        skill_lvcnofigData lv_cfg = GetCfgManage.Instance.GetCfgByNameAndId<skill_lvcnofigData>("skill_lv", m_cfg.id * 10000 + 1);
        return lv_cfg;
    }
}
