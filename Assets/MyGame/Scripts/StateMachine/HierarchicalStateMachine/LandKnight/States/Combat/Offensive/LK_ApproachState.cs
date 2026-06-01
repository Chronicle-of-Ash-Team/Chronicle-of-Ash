using UnityEngine;

public class LK_ApproachState : LK_BaseState
{
    private bool flyStarted;
    private bool usingFly;

    private Vector3 flyDestination;

    public LK_ApproachState(
        HierarchicalStateMachine machine,
        HierarchicalState parent,
        LandKnightBrain brain,
        LandKnightContext context)
        : base(machine, parent, brain, context)
    {
    }

    protected override void OnEnter()
    {
        flyStarted = false;

        var offensive =
            Parent as LK_OffensiveState;

        SkillData skill =
            offensive.CurrentSkill;

        if (skill == null || Context.target == null)
            return;

        float distance =
            Vector3.Distance(
                Brain.transform.position,
                Context.target.position);

        usingFly =
            distance < skill.MinRange;

        if (usingFly)
        {
            Context.animator.CrossFade(
                "Fly",
                0.1f);

            Context.animationEventRelay.EventRaised +=
                OnAnimationEvent;

            Vector3 dir =
                (Brain.transform.position -
                 Context.target.position).normalized;

            flyDestination =
                Context.target.position +
                dir * ((skill.MinRange + skill.MaxRange) * 0.5f);

            flyDestination.y =
                Brain.transform.position.y;
        }
        else
        {
            Context.animator.CrossFade(
                "Walk",
                0.1f);
        }
    }

    private void OnAnimationEvent(string evt)
    {
        if (evt == "FlyStart")
        {
            flyStarted = true;
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        var offensive =
            Parent as LK_OffensiveState;

        SkillData skill =
            offensive.CurrentSkill;

        if (skill == null ||
            Context.target == null)
        {
            Context.moveDirection = Vector3.zero;
            return;
        }

        if (usingFly)
        {
            if (!flyStarted)
                return;

            Vector3 toDest =
                flyDestination -
                Brain.transform.position;

            toDest.y = 0f;

            if (toDest.magnitude <= 0.2f)
            {
                Context.moveDirection =
                    Vector3.zero;

                return;
            }

            Brain.rigidbody.linearVelocity =
                toDest.normalized *
                Context.flySpeed;

            return;
        }

        Vector3 toTarget =
            Context.target.position -
            Brain.transform.position;

        toTarget.y = 0f;

        float distance =
            toTarget.magnitude;

        if (distance > skill.MaxRange)
        {
            Context.moveDirection =
                toTarget.normalized;
            //Brain.rigidbody.linearVelocity =
            //    toTarget.normalized *
            //    Context.runSpeed;
        }
        else
        {
            Context.moveDirection =
                Vector3.zero;
            //Brain.rigidbody.linearVelocity =
            //    Vector3.zero;
        }
    }

    protected override void OnExit()
    {
        Context.animationEventRelay.EventRaised -=
            OnAnimationEvent;

        Context.moveDirection =
            Vector3.zero;

        Brain.rigidbody.linearVelocity =
            Vector3.zero;
    }

    protected override HierarchicalState GetTransition()
    {
        var offensive =
            Parent as LK_OffensiveState;

        SkillData skill =
            offensive.CurrentSkill;

        if (skill == null ||
            Context.target == null)
        {
            return null;
        }

        float distance =
            Vector3.Distance(
                Brain.transform.position,
                Context.target.position);

        if (distance >= skill.MinRange &&
            distance <= skill.MaxRange)
        {
            return offensive.skillState;
        }

        return null;
    }
}