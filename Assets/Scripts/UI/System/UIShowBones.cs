
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIShowBones : UIBase
{
    private Action callBack;
    public RectTransform root;
    public Button close_btn;
    // Start is called before the first frame update
    public override void OnStart()
    {
   //     close_btn.onClick.AddListener(CloseSelf);


    }

    public void PlayBones(string bonesName,Action callBack,string animName = null)
    {
        this.callBack = callBack;
        DragonBonesController dragon = UiManager.LoadBonesByNmae(bonesName);
        dragon.transform.SetParent(root);
        dragon.transform.localScale = Vector3.one;
        dragon.transform.localPosition = Vector3.zero;
        string playAnim = "";
        if (animName != null)
        {
            playAnim = animName;
        }
        else
        {
            playAnim = dragon.armatureComponent.animation.animationNames[0]; 
        }
        dragon.PlayAnimation(playAnim, false, () =>
        {

            CloseSelf();

        });

    }
    public override void OnDestroyImp()
    {
        callBack();
    }
}