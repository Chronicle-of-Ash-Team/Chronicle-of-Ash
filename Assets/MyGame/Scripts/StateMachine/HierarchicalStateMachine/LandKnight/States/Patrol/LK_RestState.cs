using UnityEngine;

public class LK_RestState : LK_BaseState
{
    public LK_RestState(
        HierarchicalStateMachine machine,
        HierarchicalState parent,
        LandKnightBrain brain,
        LandKnightContext context)
        : base(machine, parent, brain, context)
    {
    }

    protected override void OnEnter()
    {
        Context.moveDirection = Vector3.zero;

        Context.animator.CrossFade("Idle", 0.15f);
    }

    protected override void OnUpdate(float deltaTime)
    {
        Context.patrolStamina +=
            Context.staminaRecoverRate * deltaTime;

        Context.patrolStamina =
            Mathf.Min(
                Context.patrolStamina,
                Context.maxPatrolStamina);
    }

    protected override HierarchicalState GetTransition()
    {
        var patrol = Parent as LK_PatrolState;

        if (Context.patrolStamina >=
            Context.maxPatrolStamina)
        {
            return patrol.moveToPointState;
        }

        return null;
    }
}