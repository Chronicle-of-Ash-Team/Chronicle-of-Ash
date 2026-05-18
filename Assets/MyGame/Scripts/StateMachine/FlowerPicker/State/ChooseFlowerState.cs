using UnityEngine;
using UnityEngine.AI;

public class ChooseFlowerState : IState
{
    private FlowerPicker brain;

    public ChooseFlowerState(FlowerPicker brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        Vector3 randomPos = GetRandomPoint(
            Vector3.zero,
            brain.chooseFlowerRadius
        );

        brain.currentFlowerTarget = randomPos;

        brain.FSM.ChangeState(
            new MoveToFlowerState(brain)
        );
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        //throw new System.NotImplementedException();
    }

    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 random =
                Random.insideUnitCircle * radius;

            Vector3 randomPos =
                center + new Vector3(
                    random.x,
                    0,
                    random.y
                );

            if (Vector3.Distance(center, randomPos) > 1f)
            {
                return randomPos;
            }
        }

        return center;
    }
}
