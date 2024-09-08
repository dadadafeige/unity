using System;
using System.Collections;
using UnityEngine;

public class CustomTimer : MonoBehaviour
{
    public delegate void TimerActionDelegate(int second);

    public class TimerInfo
    {
        public TimerActionDelegate timerAction;
        public float interval;
        public int maxExecutionCount;
        public Action onCompleteCallback;
        public Coroutine timerCoroutine;
        public int currentExecutionCount = 0;
        public bool isPaused = false;
        public bool isStop = false;
        public void StopCoroutie()
        {
            isStop = true;


        }
    }

    public TimerInfo timerInfo;

    public TimerInfo RegisterTimer(TimerActionDelegate action,float intervalInSeconds, int maxExecutions, Action onCompleteCallback = null)
    {
        timerInfo = new TimerInfo
        {
            timerAction = action,
            interval = intervalInSeconds,
            maxExecutionCount = maxExecutions,
            onCompleteCallback = onCompleteCallback
        };

        timerInfo.timerCoroutine = StartCoroutine(RunTimer(timerInfo));
        return timerInfo;
    }

    private IEnumerator RunTimer(TimerInfo info)
    {
       

        while (info.currentExecutionCount < info.maxExecutionCount)
        {
            if (info.isPaused)
            {
                yield return null;
            }
            else
            {
                //if (info.isStop)
                //{
                //    StopCoroutine(info.timerCoroutine); // 手动停止协程
                //}
                yield return new WaitForSeconds(info.interval);
                if (!info.isPaused)
                {
                    if (info.isStop)
                    {
                        StopCoroutine(info.timerCoroutine); // 手动停止协程
                    }
                    else
                    {
                        info.timerAction?.Invoke(info.currentExecutionCount);
                        info.currentExecutionCount++;
                    }
               
                }
            }
           
        }

        info.onCompleteCallback?.Invoke();
        StopCoroutine(info.timerCoroutine); // 手动停止协程
      
    }
}
