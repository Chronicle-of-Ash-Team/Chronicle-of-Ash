using UnityEngine;

public class StandUpState : IState
{
    private FlowerPicker brain;

    private float timer;

    public StandUpState(FlowerPicker brain)
    {
        this.brain = brain;
    }
    public void OnEnter()
    {
        timer = brain.standUpTime;
        brain.animator.CrossFade("StandUp", 0.1f);
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
