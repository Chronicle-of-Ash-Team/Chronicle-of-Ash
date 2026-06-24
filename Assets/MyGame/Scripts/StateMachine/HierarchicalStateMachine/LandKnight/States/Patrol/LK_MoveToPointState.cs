using UnityEngine;

public class LK_MoveToPointState : LK_BaseState
{
    private Vector3 targetPoint;

    public LK_MoveToPointState(
        HierarchicalStateMachine machine,
        HierarchicalState parent,
        LandKnightBrain brain,
        LandKnightContext context)
        : base(machine, parent, brain, context)
    {
    }

    protected override void OnEnter()
    {
        PickNewPoint();

        Context.animator.CrossFade(
            "Walk",
            0.2f);
    }

    protected override void OnUpdate(float deltaTime)
    {
        Context.patrolStamina -=
            Context.staminaDrainRate *
            deltaTime;

        Vector3 toTarget =
            targetPoint -
            Brain.transform.position;

        toTarget.y = 0f;

        Context.moveDirection =
            toTarget.normalized;
    }

    protected override HierarchicalState GetTransition()
    {
        var patrol =
            Parent as LK_PatrolState;

        if (Context.patrolStamina <= 0f)
        {
            return patrol.restState;
        }

        Vector3 toTarget =
            targetPoint -
            Brain.transform.position;

        toTarget.y = 0f;

        if (toTarget.magnitude <= 0.5f)
        {
            return patrol.waitState;
        }

        return null;
    }

    protected override void OnExit()
    {
        Context.moveDirection =
            Vector3.zero;
    }

    private void PickNewPoint()
    {
        Vector2 random =
            Random.insideUnitCircle *
            Context.patrolRange;

        targetPoint =
            Context.patrolCenter +
            new Vector3(
                random.x,
                0f,
                random.y);
    }
}