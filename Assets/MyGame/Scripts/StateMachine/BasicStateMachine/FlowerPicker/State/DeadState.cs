using UnityEngine;

public class DeadState : IState
{
    private FlowerPicker brain;

    public DeadState(FlowerPicker brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        brain.animator.CrossFade("Death", 0.2f);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
