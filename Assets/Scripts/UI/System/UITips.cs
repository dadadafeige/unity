
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UITips : UIBase
{
    public RectTransform mTran;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI mLabel;

    public override void OnAwake()
    {

    }
    // Start is called before the first frame update
    public override void OnStart()
    {

        // goBtn.onClick.AddListener(ButtonOnClickEvent);
        Tween tween = mTran.DOMoveY(mTran.position.y + 2, 2);
        tween.SetEase(Ease.OutCubic);
        tween.onComplete = () =>
        {
            //canvasGroup.alpha
            Tween tween1 = canvasGroup.DOFade(0, 1);
            tween1.onComplete = () =>
            {

                CloseSelf();

            };

        };
    }
    float timer = 0.8f;
    float timerEx = 0;
    void Update()
    {
        //timer -= Time.deltaTime;
        //mTran.localPosition = Vector3.Lerp(mTran.localPosition, new Vector3(mTran.localPosition.x, mTran.localPosition.y + 200,0) , timer/40);
        //timerEx += Time.deltaTime;
        //canvasGroup.alpha = Mathf.Lerp(1, 0, timerEx/3);
        //if (timerEx >= 3)
        //{
        //    CloseSelf();
        //}


    }
    public void SetLabel(string str)
    {
        mLabel.text = str;

    }

}