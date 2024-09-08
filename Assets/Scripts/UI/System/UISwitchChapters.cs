using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UISwitchChapters : UIBase
{

    public RawImage transitionImage;
    public TextMeshProUGUI transitionText;
    public TextMeshProUGUI nameText;
    public Button btn;
    public override void OnStart()
    {
        // 初始化 UI 状态
        InitializeUI();

        // 执行过场动画
        PlayTransitionAnimation();
        btn.onClick.AddListener(() =>
        {
            //Tween tween = transitionImage.DOFade(0f, 1.5f);
            //tween.onComplete = () =>
            //{
            //    CloseSelf();
            //};
            CloseSelf();
        });
    }

    void InitializeUI()
    {
        chapterconfigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<chapterconfigData>("chapter", GameManage.userData.unlockChapter);
        //transitionText.text = cfg.chapter_dec;
        //nameText.text = cfg.name;
        transitionImage.texture = UiManager.getTextureByNmae("switch_chapters_texture", cfg.chapter_texture);
        // 设置初始状态
        transitionImage.color = new Color(1f, 1f, 1f, 0f); // 初始时透明
        //transitionText.color = new Color(0f, 0f, 0f, 0f); // 初始时透明
        //nameText.color = new Color(0f, 0f, 0f, 0f); // 初始时透明
    }

    void PlayTransitionAnimation()
    {

        transitionImage.DOFade(1f, 1.5f);





        // 通过 DOTween 库执行 Tween 动画
     //   DG.Tweening.Sequence sequence = DOTween.Sequence();

        // 图片渐显动画
        // sequence.Append(transitionImage.DOFade(1f, 1.5f));

        // 文字逐个字出现动画
        //for (int i = 0; i < transitionText.text.Length; i++)
        //{
        //    int currentIndex = i; // 保存当前索引以避免闭包问题
        //    sequence.Join(transitionText.DOText(transitionText.text.Substring(0, currentIndex + 1), 0.5f)
        //        .SetDelay(0.1f * currentIndex) // 延迟时间，可以调整
        //        .SetEase(Ease.Linear));
        //}

        // 文字渐显动画，并向上移动
        //sequence.Join(transitionText.rectTransform.DOAnchorPosY(50f, 1.5f));
        // sequence.Join(transitionText.DOFade(1f, 1.5f));
        //sequence.Join(nameText.DOFade(1f, 1.5f));
        //// 停顿一段时间
        //sequence.AppendInterval(2f);

        // 执行渐隐动画
        // sequence.Append(transitionImage.DOFade(0f, 1.5f));
        // sequence.Join(transitionText.DOFade(0f, 1.5f));
        //   sequence.Join(nameText.DOFade(0f, 1.5f));
        //   sequence.Join(transitionText.rectTransform.DOAnchorPosY(100f, 1.5f));

        // 完成后可以添加回调函数，执行其他逻辑
        //sequence.OnComplete(() =>
        //{
        //    CloseSelf();
        //});
    }
    private string targetText;
    private float letterDelay = 0.1f;
    IEnumerator ShowText()
    {
        foreach (char letter in targetText)
        {
            transitionText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }




}


