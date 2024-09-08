// 状态机基类
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StateMachine
{
    public Action OnStateChange; // 状态改变时的事件

    private IState currentState;

    // 设置当前状态
    public void SetState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();

        OnStateChange?.Invoke();
    }

    // 更新当前状态
    public void Update()
    {
        currentState?.Update();
    }
}