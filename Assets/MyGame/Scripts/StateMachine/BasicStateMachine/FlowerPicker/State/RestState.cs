using UnityEngine;

public class RestState : IState
{
    private FlowerPicker brain;

    private float timer;

    public RestState(FlowerPicker brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        timer = brain.restTime;
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            brain.FSM.ChangeState(brain.chooseFlowerState);
        }
    }
}
