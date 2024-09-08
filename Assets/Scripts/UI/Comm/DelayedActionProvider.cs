using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DelayedActionProvider : MonoBehaviour
{
    private class DelayedActionData
    {
        public Action Action;
        public float DelayInSeconds;
    }

    private Coroutine delayedActionsCoroutine;
    private readonly Queue<DelayedActionData> delayedActionsQueue = new Queue<DelayedActionData>();
    // ����ʵ��
    private static DelayedActionProvider instance;

    // ��ȡ����ʵ��������
    public static DelayedActionProvider Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DelayedActionProvider>();

                if (instance == null)
                {
                    GameObject delayedActionProviderObject = new GameObject("DelayedActionProvider");
                    instance = delayedActionProviderObject.AddComponent<DelayedActionProvider>();
                   
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        // ȷ��ֻ��һ��ʵ������
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public DelayedCoroutineData DelayedAction(Action action, float delayInSeconds)
    {
        DelayedCoroutineData delayed = new DelayedCoroutineData();
        Coroutine coroutine = StartCoroutine(DelayedExecution(action, delayInSeconds, delayed));
        delayed.coroutine = coroutine;
        return delayed;
    }

    private IEnumerator DelayedExecution(Action action, float delayInSeconds, DelayedCoroutineData delayed)
    {
        yield return new WaitForSeconds(delayInSeconds);
        if (delayed.isStop)
        {
            StopCoroutine(delayed.coroutine);
        }
        else
        {
            action?.Invoke();
        }
       
    }
}
public class DelayedCoroutineData
{
    public Coroutine coroutine;
    public bool isStop = false;
 

}