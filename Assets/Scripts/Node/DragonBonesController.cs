using UnityEngine;
using DragonBones;
using System;

public class DragonBonesController : MonoBehaviour
{
    // DragonBones �����������
    public UnityArmatureComponent armatureComponent;

    // �Ƿ�����ͣ״̬
    private bool isPaused = false;

    // �Ƿ�ѭ������
    public bool isLooping = true;
    private Action action = null;
    public bool isInitAni = false;

    void Start()
    {
        if (isInitAni)
        {
            // ����Ĭ�ϲ��ŵĶ��������Ը���ʵ���������ã�
            PlayAnimation(armatureComponent.animation.animationNames[0], isLooping);

        }
 

        // ע�Ქ����ϻص�
        armatureComponent.AddDBEventListener(EventObject.COMPLETE, OnAnimationComplete);
    }

    // ����ָ�����ƵĶ���
    public void PlayAnimation(string animationName, bool loop = false,Action action = null)
    {
        if (armatureComponent != null)
        {
   
            isPaused = false; // ����ʱȡ����ͣ״̬
            isLooping = loop; // ����ѭ��״̬
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

    // �л���ָ�����ƵĶ���������
    public void SwitchAndPlayAnimation(string animationName, bool loop = false,Action action = null)
    {
        if (armatureComponent != null)
        {
            // �л���ָ���Ķ���
            armatureComponent.animation.GotoAndPlayByTime(animationName);
            isPaused = false; // �л�ʱȡ����ͣ״̬
            isLooping = loop; // ����ѭ��״̬
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

    // ��ͣ��������
    public void PauseAnimation()
    {
        if (armatureComponent != null && !isPaused)
        {
            armatureComponent.animation.Stop();
            isPaused = true;
        }
    }

    // ������������
    public void ResumeAnimation()
    {
        if (armatureComponent != null && isPaused)
        {
            armatureComponent.animation.Play();
            isPaused = false;
        }
    }

    // ������ϻص�
    private void OnAnimationComplete(string type, EventObject eventObject)
    {
        Debug.Log("Animation Complete: " + eventObject.animationState.name);
        // �����������Ļص��߼�

        // �����ѭ�����ŵĶ��������������ﴦ��ÿ�β�����Ϻ���߼�

        // �������ѭ�����ţ�����ѡ��������ȡ���¼���������ֹ�ٴδ���
        if (action != null)
        {
            action();
        }

    }

    // �ڽű�����ʱ�Ƴ��¼���������ֹ�ڴ�й©
    private void OnDestroy()
    {
        if (armatureComponent != null)
        {
            armatureComponent.RemoveDBEventListener(EventObject.COMPLETE, OnAnimationComplete);
        }
    }
}
