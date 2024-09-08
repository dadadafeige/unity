// ״̬������
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StateMachine
{
    public Action OnStateChange; // ״̬�ı�ʱ���¼�

    private IState currentState;

    // ���õ�ǰ״̬
    public void SetState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();

        OnStateChange?.Invoke();
    }

    // ���µ�ǰ״̬
    public void Update()
    {
        currentState?.Update();
    }
}