using UnityEngine;

public class LK_StaggerState : LK_BaseState
{
    private bool staggerFinished;

    public LK_StaggerState(
        HierarchicalStateMachine machine,
        HierarchicalState parent,
        LandKnightBrain brain,
        LandKnightContext context)
        : base(machine, parent, brain, context)
    {
    }

    protected override void OnEnter()
    {
        staggerFinished = false;

        Context.postureBroken = false;

        Context.posture = 0f;

        Context.moveDirection = Vector3.zero;

        Brain.rigidbody.linearVelocity =
            Vector3.zero;

        Context.animator.CrossFade(
            "Stun",
            0.05f);

        ApplyKnockback();

        WaitStaggerEnd();
    }

    protected override void OnUpdate(float deltaTime)
    {
        Vector3 velocity =
            Brain.rigidbody.linearVelocity;

        velocity.x = Mathf.Lerp(
            velocity.x,
            0f,
            4f * deltaTime);

        velocity.z = Mathf.Lerp(
            velocity.z,
            0f,
            4f * deltaTime);

        Brain.rigidbody.linearVelocity =
            velocity;
    }

    protected override void OnExit()
    {
        Context.posture = 0f;

        Context.moveDirection = Vector3.zero;

        Vector3 velocity =
            Brain.rigidbody.linearVelocity;

        velocity.x = 0f;
        velocity.z = 0f;

        Brain.rigidbody.linearVelocity =
            velocity;
    }

    protected override HierarchicalState GetTransition()
    {
        if (!staggerFinished)
            return null;

        var root = Parent as LK_RootState;

        return root.patrolState;
    }

    private async void WaitStaggerEnd()
    {
        await Context.animationEventRelay
            .WaitEvent("StaggerEnd");

        staggerFinished = true;
    }

    private void ApplyKnockback()
    {
        Vector3 dir =
            Context.staggerDirection;

        dir.y = 0f;

        if (dir.sqrMagnitude <= 0.001f)
            return;

        dir.Normalize();

        Vector3 knockback =
            dir * Context.staggerForce;

        Vector3 velocity =
            Brain.rigidbody.linearVelocity;

        velocity += knockback;

        Brain.rigidbody.linearVelocity =
            velocity;
    }
}