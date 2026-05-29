using UnityEngine;

public class LK_WaitState : LK_BaseState
{
    private float timer;
    private float duration;

    public LK_WaitState(
        HierarchicalStateMachine machine,
        HierarchicalState parent,
        LandKnightBrain brain,
        LandKnightContext context)
        : base(machine, parent, brain, context)
    {
    }

    protected override void OnEnter()
    {
        timer = 0f;

        duration =
            Random.Range(2f, 5f);

        Context.moveDirection =
            Vector3.zero;

        Context.animator.CrossFade(
            "LookAround",
            0.2f);
    }

    protected override void OnUpdate(float deltaTime)
    {
        timer += deltaTime;

        // hồi stamina khi nghỉ
        Context.patrolStamina +=
            Context.staminaRecoverRate *
            deltaTime;

        Context.patrolStamina =
            Mathf.Min(
                Context.patrolStamina,
                Context.maxPatrolStamina);
    }

    protected override HierarchicalState GetTransition()
    {
        var patrol =
            Parent as LK_PatrolState;

        if (timer >= duration)
        {
            return patrol.moveToPointState;
        }

        return null;
    }

    protected override void OnExit()
    {
        Context.moveDirection =
            Vector3.zero;
    }
}