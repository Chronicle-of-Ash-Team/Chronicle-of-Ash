using UnityEngine;

public class MoveToFlowerState : IState
{
    private FlowerPicker brain;

    private Vector3 currentMoveDir = Vector3.zero;
        
    public MoveToFlowerState(FlowerPicker brain)
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
        float distance = Vector2.Distance(
            new Vector2(
                brain.transform.position.x,
                brain.transform.position.z
            ),
            new Vector2(
                brain.currentFlowerTarget.x,
                brain.currentFlowerTarget.z
            )
        );

        if (distance > brain.pickingDistance)
        {
            Vector3 moveDir = GetMoveDirToTarget(brain.transform, brain.currentFlowerTarget);
            HandleMovement(moveDir);
        }
        else
        {
            brain.FSM.ChangeState(new PickFlowerState(brain));
        }
    }

    private void HandleMovement(Vector3 moveDir)
    {
        currentMoveDir = moveDir;
        currentMoveDir.y = 0f;

        Vector3 velocity = currentMoveDir * brain.moveSpeed;
        velocity.y = brain.rigidbody.linearVelocity.y;
        brain.rigidbody.linearVelocity = velocity;
    }

    private Vector3 GetMoveDirToTarget(Transform self, Vector3 target)
    {
        Vector3 dir = target - self.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return Vector3.zero;

        return dir.normalized;
    }
}
