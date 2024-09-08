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
        // ��ʼ�� UI ״̬
        InitializeUI();

        // ִ�й�������
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
        // ���ó�ʼ״̬
        transitionImage.color = new Color(1f, 1f, 1f, 0f); // ��ʼʱ͸��
        //transitionText.color = new Color(0f, 0f, 0f, 0f); // ��ʼʱ͸��
        //nameText.color = new Color(0f, 0f, 0f, 0f); // ��ʼʱ͸��
    }

    void PlayTransitionAnimation()
    {

        transitionImage.DOFade(1f, 1.5f);





        // ͨ�� DOTween ��ִ�� Tween ����
     //   DG.Tweening.Sequence sequence = DOTween.Sequence();

        // ͼƬ���Զ���
        // sequence.Append(transitionImage.DOFade(1f, 1.5f));

        // ��������ֳ��ֶ���
        //for (int i = 0; i < transitionText.text.Length; i++)
        //{
        //    int currentIndex = i; // ���浱ǰ�����Ա���հ�����
        //    sequence.Join(transitionText.DOText(transitionText.text.Substring(0, currentIndex + 1), 0.5f)
        //        .SetDelay(0.1f * currentIndex) // �ӳ�ʱ�䣬���Ե���
        //        .SetEase(Ease.Linear));
        //}

        // ���ֽ��Զ������������ƶ�
        //sequence.Join(transitionText.rectTransform.DOAnchorPosY(50f, 1.5f));
        // sequence.Join(transitionText.DOFade(1f, 1.5f));
        //sequence.Join(nameText.DOFade(1f, 1.5f));
        //// ͣ��һ��ʱ��
        //sequence.AppendInterval(2f);

        // ִ�н�������
        // sequence.Append(transitionImage.DOFade(0f, 1.5f));
        // sequence.Join(transitionText.DOFade(0f, 1.5f));
        //   sequence.Join(nameText.DOFade(0f, 1.5f));
        //   sequence.Join(transitionText.rectTransform.DOAnchorPosY(100f, 1.5f));

        // ��ɺ������ӻص�������ִ�������߼�
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


