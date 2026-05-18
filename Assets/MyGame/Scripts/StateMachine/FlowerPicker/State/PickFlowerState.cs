using UnityEngine;

public class PickFlowerState : IState
{
    private FlowerPicker brain;

    private float pickFlowerTimer = 0f;

    public PickFlowerState(FlowerPicker brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        pickFlowerTimer = brain.pickFlowerTime;
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        pickFlowerTimer -= Time.deltaTime;

        if(pickFlowerTimer < 0f)
        {
            brain.FSM.ChangeState(new RestState(brain));
        }
    }
}
