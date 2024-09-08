using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGetReward : UIBase
{
    public DragonBonesController dragon;
    public RectTransform rect;
    public GameObject mask;
    public Image icon;
    public override void OnAwake()
    {
      
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
     //   mask.SetActive(false);


    }
    public void SetData(Transform tran,Action action = null)
    {
        dragon.PlayAnimation(dragon.armatureComponent.animation.animationNames[0], false, () =>
        {
            rect.DOMove(tran.position, 0.5f);
            Tween tween = rect.DOScale(0, 0.5f);
            UnityEngine.Vector3 vector = tran.localScale;
            tween.onComplete = () =>
            {
                Tween tween1 = tran.DOScale(vector * 1.5f, 0.25f);
                tween1.onComplete = () =>
                {
                    tran.DOScale(vector, 0.25f);
                };
                if (action != null)
                {
                    action();
                }
                CloseSelf();
            };
           
        });
       

        }
    public void SetData(Transform tran, int itemId, Action action = null)
    {
        itmeconfigData mCfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", itemId);
        icon.sprite = UiManager.LoadSprite("item_icon", mCfg.icon);
        icon.SetNativeSize();
        dragon.PlayAnimation(dragon.armatureComponent.animation.animationNames[0], false, () =>
        {
            rect.DOMove(tran.position, 0.5f);
            Tween tween = rect.DOScale(0, 0.5f);
            tween.onComplete = () =>
            {
                UnityEngine.Vector3 vector = tran.localScale;
                Tween tween1 = tran.DOScale(vector * 1.5f, 0.25f);
                tween1.onComplete = () =>
                {
                    tran.DOScale(vector, 0.25f);
                };
                if (action != null)
                {
                    action();
                }
                CloseSelf();
            };

        });
    }
    public void SetData(Transform tran, Sprite sprite, Action action = null)
    {
        icon.sprite = sprite;
        icon.SetNativeSize();
        icon.transform.localRotation = Quaternion.identity;
        icon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        dragon.PlayAnimation(dragon.armatureComponent.animation.animationNames[0], false, () =>
        {
            rect.DOMove(tran.position, 0.5f);
            Tween tween = rect.DOScale(0, 0.5f);
            tween.onComplete = () =>
            {
                UnityEngine.Vector3 vector = tran.localScale;
                Tween tween1 = tran.DOScale(vector * 1.5f, 0.25f);
                tween1.onComplete = () =>
                {
                    tran.DOScale(vector, 0.25f);
                };
                if (action != null)
                {
                    action();
                }
                CloseSelf();
            };

        });
    }

}
