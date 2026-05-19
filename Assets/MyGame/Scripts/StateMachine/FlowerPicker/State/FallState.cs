using UnityEngine;

public class FallState : IState
{
    private FlowerPicker brain;

    private float timer;

    public FallState(FlowerPicker brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        brain.animator.Play("Tripping");
        timer = brain.fallTime;
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            brain.FSM.ChangeState(brain.restState);
        }
    }
}
