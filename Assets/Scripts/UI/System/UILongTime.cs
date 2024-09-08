using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILongTime : UIBase
{
    public Image bg;
    public Image long_time;
    public CanvasGroup canvasGroup;
    // Start is called before the first frame update
    public override void OnStart()
    {
        long_time.color = new Color(1.5f, 1f, 1f, 0f);
        long_time.DOFade(1, 1);
        DOVirtual.DelayedCall(3f, () =>
        {
            canvasGroup.DOFade(0, 1.5f).onComplete = () =>
            {

                CloseSelf();
            };

        });
        // Update is called once per frame
    }
}
