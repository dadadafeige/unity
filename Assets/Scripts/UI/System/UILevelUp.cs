using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUp : UIBase
{
    public TextMeshProUGUI lv;
    public TextMeshProUGUI tolv;
    public TextMeshProUGUI to_attack;
    public TextMeshProUGUI to_defense;
    public TextMeshProUGUI to_magic_resistance;
    public TextMeshProUGUI to_hp;
    public DragonBonesController dragon;
    public Button mask;
    // Start is called before the first frame update
    public override void OnStart()
    {
        dragon.PlayAnimation("01_Go", false, () =>
        {
            dragon.PlayAnimation("02_Idle", false);

        });
        mask.onClick.AddListener(CloseSelf);
    }
    public void SetData(int curlv)
    {
        lv.text = "·ûÊ¦µÈ¼¶" + (curlv - 1);
        tolv.text = curlv.ToString();
        player_attributecnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<player_attributecnofigData>("player_attribute", curlv);
        to_attack.text = cfg.attack.ToString();
        to_defense.text = cfg.defense.ToString();
        to_magic_resistance.text = cfg.magic_resistance.ToString();
        to_hp.text = cfg.hp.ToString();
    }
}
