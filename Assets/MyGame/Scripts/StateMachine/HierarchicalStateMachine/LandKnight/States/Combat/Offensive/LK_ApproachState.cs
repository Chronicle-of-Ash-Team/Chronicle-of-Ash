using UnityEngine;

public class LK_ApproachState : LK_BaseState
{
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
        Context.animator.CrossFade(
            "Walk",
            0.2f);

        var offensive =
            Parent as LK_OffensiveState;

        SkillData skill =
            offensive.CurrentSkill;

        Debug.Log("Enter Approach State with skill: " + (skill != null ? skill.AnimationName : "None"));
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
            Context.moveDirection =
                Vector3.zero;
            return;
        }

        Vector3 toTarget =
            Context.target.position -
            Brain.transform.position;

        toTarget.y = 0f;

        float distance =
            toTarget.magnitude;

        Vector3 dir =
            toTarget.normalized;

        // Quá xa -> tiến vào
        if (distance > skill.MaxRange)
        {
            Context.moveDirection = dir;
        }
        // Quá gần -> lùi ra
        else if (distance < skill.MinRange)
        {
            Context.moveDirection = -dir;
        }
        // Đúng vị trí
        else
        {
            Context.moveDirection = Vector3.zero;
        }
    }

    protected override void OnExit()
    {
        Context.moveDirection =
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