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

    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
