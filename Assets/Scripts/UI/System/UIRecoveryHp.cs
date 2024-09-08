using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecoveryHp : UIBase
{
    public Image go_attack_pro;
    public Button go_attack_pro_btn;
    public Button close_btn;
    private Tween tween;
    private Action<float> callBack;
    public Image pro1;
    public Image pro2;
    public TextMeshProUGUI add1;
    public TextMeshProUGUI add2;
    public DragonBonesController dragon;
    public GameObject panlock;
    private float curHp = 0;
    private float maxHp = 50;
    public Button rule_btn;
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

        MissionManage.ShowDescription();
        rule_btn.onClick.AddListener(() =>
        {

            MissionManage.ShowDescription();

        });

    }
    public void SetCallBack(Action<float> action)
    {

        callBack = action;

    }
    private void OnClickGoAttackBtn()
    {

        tween.Pause();
        float add_num = 0;
        if (go_attack_pro.fillAmount > 0.89)
        {
            add_num = 20;
        }
        else if (go_attack_pro.fillAmount > 0.79)
        {
            add_num = 10;
        }
        if (add_num > maxHp - curHp)
        {
            add_num = maxHp - curHp;
        }
        add1.text = "+"+add_num.ToString();
        add2.text = "+"+add_num.ToString();
        add1.gameObject.SetActive(true);
        add2.gameObject.SetActive(true);
        panlock.SetActive(true);
        Vector3 addPos1 = add1.transform.localPosition;
        Vector3 addPos2 = add2.transform.localPosition;
        add1.transform.DOLocalMoveY(addPos1.y + 300, 1.3f);
        Tween tween1 = add2.transform.DOLocalMoveY(addPos2.y + 300, 1.3f);
        tween1.onComplete = () => {
            add1.gameObject.SetActive(false);
            add2.gameObject.SetActive(false);
            add1.transform.localPosition = addPos1;
            add2.transform.localPosition = addPos2;
            if (add_num == 0)
            {
                panlock.SetActive(false);
                tween.Play();
            }
        };
       

        if (add_num > 0)
        {
            curHp = curHp + add_num;
            pro1.DOFillAmount(curHp / maxHp , 1);
            pro2.DOFillAmount(curHp / maxHp, 1);
            dragon.gameObject.SetActive(true);

            dragon.PlayAnimation("01_attack", false, () =>
            {
                dragon.gameObject.SetActive(false);
                panlock.SetActive(false);
                tween.Play();
                if (maxHp == curHp)
                {
                    UIFeedingFishSettle gui = Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(1), () =>
                    {


                    }, () => {
                        CloseSelf();
                    }, () =>
                    {


                    });
                    gui.come_back_btn.gameObject.SetActive(false);

                }

            });
        }

       // CloseSelf();
    }
}
