using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleSettle : UIBase
{

    public Image result;
    public Button mask;
    public TextMeshProUGUI expLabel;
    public Button close_btn;

    public override void OnAwake()
    {
      
    }
    // Start is called before the first frame update
    public override void OnStart()
    {





    }
    public void SetData(bool isSuccess, Action action)
    {
        expLabel.text ="X"+ BattleManager.Instance.addExp;
        GameManage.userData.SetAddExpValue(BattleManager.Instance.addExp);
        mask.onClick.AddListener(() =>
        {
            action();
            CloseSelf();

        });
        close_btn.onClick.AddListener(() =>
        {
            action();
            CloseSelf();

        });
        if (isSuccess)
        {
            result.sprite = UiManager.LoadSprite("common", "get_reward");
        }
        else
        {
            result.sprite = UiManager.LoadSprite("common", "fail_word");
        }
        result.SetNativeSize();
    }

}
