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
        brain.animator.CrossFade("PickFlower", 0.2f);
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException();
        brain.animator.CrossFade("Idle", 0.5f);
    }

    public void OnUpdate()
    {
        pickFlowerTimer -= Time.deltaTime;

        if(pickFlowerTimer < 0f)
        {
            brain.FSM.ChangeState(brain.restState);
        }
    }
}
