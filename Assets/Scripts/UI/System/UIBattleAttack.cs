using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleAttack : UIBase
{
    public Image go_attack_pro;
    public Button go_attack_pro_btn;
    public Button close_btn;
    private Tween tween;
    private Action<float> callBack;
    public override void OnAwake()
    {
      
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        close_btn.onClick.AddListener(CloseSelf);
        tween = go_attack_pro.DOFillAmount(0, 0.75f);
        tween.SetLoops(-1, LoopType.Yoyo);
        go_attack_pro_btn.onClick.AddListener(OnClickGoAttackBtn);


    }
    public void SetCallBack(Action<float> action)
    {

        callBack = action;

    }
    private void OnClickGoAttackBtn()
    {

        tween.Kill();
        callBack(go_attack_pro.fillAmount);
        CloseSelf();
    }
}
