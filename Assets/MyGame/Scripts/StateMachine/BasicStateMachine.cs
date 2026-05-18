using UnityEngine;

public class BasicStateMachine
{
    public IState currentState { get; private set; }

    public void ChangeState(IState state)
    {
        currentState?.OnExit();

        currentState = state;

        currentState.OnEnter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }
}
