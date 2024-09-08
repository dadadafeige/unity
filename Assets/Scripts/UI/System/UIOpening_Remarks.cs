using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIOpening_Remarks : UIBase
{
    public RawImage rawImage1;
    public RawImage rawImage2;
    public Button skip_btn;
    public Button btn;
    private string[] strs = new string[7] {
        "KaiChang_01",
        "KaiChang_02",
        "KaiChang_03",
        "KaiChang_04",
        "KaiChang_05",
        "KaiChang_06",
        "KaiChang_07",
       
    };
    private float fadeDuration = 2.0f; // 渐显和渐隐的持续时间
    private float displayDuration = 2.0f; // 每张图片显示的持续时间
    private int currentIndex = 0;
    private bool isRawImage1Active = false;
    Tween tween;

    // Start is called before the first frame update
    public override void OnStart()
    {
        //rawImage2.texture = UiManager.getTextureByNmae("opening_remarks", strs[1]);
        //rawImage2.transform.SetAsFirstSibling();

        Invoke("StartFadeSequence", displayDuration);
        btn.onClick.AddListener(() =>
        {
            CancelInvoke("StartFadeSequence");
            StartFadeSequence();
        });
        skip_btn.onClick.AddListener(()=> {
            UIPlayerStory gui = UiManager.OpenUI<UIPlayerStory>("UIPlayerStory");
            UiManager.uIPlayer = gui;
            CloseSelf();
        });
    }
    void StartFadeSequence()
    {
    
        currentIndex++;
        //equence sequence = DOTween.Sequence();
        if (currentIndex == 7)
        {
            UIPlayerStory gui = UiManager.OpenUI<UIPlayerStory>("UIPlayerStory");
            UiManager.uIPlayer = gui;
            CloseSelf();
            return;
        }
        RawImage raw;

        if (isRawImage1Active)
        {

            rawImage2.transform.SetAsFirstSibling();
            rawImage2.texture = UiManager.getTextureByNmae("opening_remarks", strs[currentIndex]);
 
            raw = rawImage1;
        }
        else
        {
            rawImage1.transform.SetAsFirstSibling();
            rawImage1.texture = UiManager.getTextureByNmae("opening_remarks", strs[currentIndex]);
        
            raw = rawImage2;
        }
        rawImage1.color = new Color(1f, 1f, 1f, 1f); // 初始时透明
        rawImage2.color = new Color(1f, 1f, 1f, 1f); // 初始时透明
        if (tween != null)
        {
            tween.Kill();
            tween = null;

        }
        tween = raw.DOFade(0, fadeDuration);
        tween.onComplete = () =>
        {
            isRawImage1Active = !isRawImage1Active;
            Invoke("StartFadeSequence", displayDuration);
            // StartFadeSequence();
            
 
        };


        return;


        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < strs.Length - 1; i++)
        {
          



            sequence.AppendCallback(() => { currentIndex++; SwapImages(); });
            sequence.Append(isRawImage1Active ? rawImage2.DOFade(0, fadeDuration) :  rawImage1.DOFade(0, fadeDuration));
            sequence.AppendCallback(() => {

                if (isRawImage1Active)
                {
                    rawImage2.transform.SetAsFirstSibling();


                }
                else
                {

                    rawImage1.transform.SetAsFirstSibling();

                }

            });
            sequence.AppendInterval(displayDuration);
            

        }
        sequence.AppendCallback(() => { CloseSelf(); });
        // sequence.SetLoops(-1); // 循环播放


        //   sequence.SetLoops(-1); // 循环播放
    }
    void SwapImages()
    {
        if (isRawImage1Active)
        {

            rawImage2.texture = UiManager.getTextureByNmae("opening_remarks", strs[currentIndex]);
            rawImage1.color = new Color(1f, 1f, 1f, 1f); // 初始时透明
            rawImage2.color = new Color(1f, 1f, 1f, 1f); // 初始时透明
         
        }
        else
        {

            rawImage1.texture = UiManager.getTextureByNmae("opening_remarks", strs[currentIndex]);
            //rawImage2.texture = UiManager.getTextureByNmae("opening_remarks", strs[currentIndex]);
            rawImage2.color = new Color(1f, 1f, 1f, 1f); // 初始时透明
            rawImage1.color = new Color(1f, 1f, 1f, 1f); // 初始时透明
         
   
        }
        isRawImage1Active = !isRawImage1Active;
    }
    public override void OnDestroyImp()
    {
        CancelInvoke("StartFadeSequence");
    }
}
