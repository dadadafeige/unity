using UnityEngine;
using DragonBones;
using System;

public class DragonBonesController : MonoBehaviour
{
    // DragonBones 骨骼动画组件
    public UnityArmatureComponent armatureComponent;

    // 是否处于暂停状态
    private bool isPaused = false;

    // 是否循环播放
    public bool isLooping = true;
    private Action action = null;
    public bool isInitAni = false;

    void Start()
    {
        if (isInitAni)
        {
            // 设置默认播放的动画（可以根据实际需求设置）
            PlayAnimation(armatureComponent.animation.animationNames[0], isLooping);

        }
 

        // 注册播放完毕回调
        armatureComponent.AddDBEventListener(EventObject.COMPLETE, OnAnimationComplete);
    }

    // 播放指定名称的动画
    public void PlayAnimation(string animationName, bool loop = false,Action action = null)
    {
        if (armatureComponent != null)
        {
   
            isPaused = false; // 播放时取消暂停状态
            isLooping = loop; // 设置循环状态
            if (!loop)
            {
                armatureComponent.animation.Play(animationName,1);

            }
            else
            {
                armatureComponent.animation.Play(animationName);
            }
            this.action = action;
        }
    }

    // 切换到指定名称的动画并播放
    public void SwitchAndPlayAnimation(string animationName, bool loop = false,Action action = null)
    {
        if (armatureComponent != null)
        {
            // 切换到指定的动画
            armatureComponent.animation.GotoAndPlayByTime(animationName);
            isPaused = false; // 切换时取消暂停状态
            isLooping = loop; // 设置循环状态
            if (!loop)
            {
                armatureComponent.animation.GotoAndPlayByTime(animationName,0,1);
            }
            else
            {
                armatureComponent.animation.GotoAndPlayByTime(animationName);
            }
        }
        this.action = action;
    }

    // 暂停动画播放
    public void PauseAnimation()
    {
        if (armatureComponent != null && !isPaused)
        {
            armatureComponent.animation.Stop();
            isPaused = true;
        }
    }

    // 继续动画播放
    public void ResumeAnimation()
    {
        if (armatureComponent != null && isPaused)
        {
            armatureComponent.animation.Play();
            isPaused = false;
        }
    }

    // 播放完毕回调
    private void OnAnimationComplete(string type, EventObject eventObject)
    {
        Debug.Log("Animation Complete: " + eventObject.animationState.name);
        // 在这里添加你的回调逻辑

        // 如果是循环播放的动画，可以在这里处理每次播放完毕后的逻辑

        // 如果不是循环播放，可以选择在这里取消事件监听，防止再次触发
        if (action != null)
        {
            action();
        }

    }

    // 在脚本销毁时移除事件监听，防止内存泄漏
    private void OnDestroy()
    {
        if (armatureComponent != null)
        {
            armatureComponent.RemoveDBEventListener(EventObject.COMPLETE, OnAnimationComplete);
        }
    }
}
