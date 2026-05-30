using UnityEngine;

public class LK_SkillState : LK_BaseState
{
    private bool finished;

    public LK_SkillState(
        HierarchicalStateMachine machine,
        HierarchicalState parent,
        LandKnightBrain brain,
        LandKnightContext context)
        : base(machine, parent, brain, context)
    {
    }

    protected override void OnEnter()
    {
        finished = false;

        var offensive =
            Parent as LK_OffensiveState;

        SkillData skill =
            offensive.CurrentSkill;

        if (skill == null)
        {
            finished = true;
            return;
        }

        Context.moveDirection =
            Vector3.zero;

        Context.animator.CrossFade(
            skill.AnimationName,
            0.05f);

        Context.animationEventRelay.EventRaised +=
            OnAnimationEvent;

        Vector3 dir =
            Context.target.position -
            Brain.transform.position;

        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Brain.transform.rotation =
                Quaternion.LookRotation(
                    dir.normalized);
        }
    }

    protected override void OnExit()
    {
        Context.animationEventRelay.EventRaised -=
            OnAnimationEvent;

        Context.moveDirection =
            Vector3.zero;
    }

    private void OnAnimationEvent(string evt)
    {
        if (evt == "SkillEnd")
        {
            finished = true;
        }

        var offensive =
            Parent as LK_OffensiveState;

        switch (offensive.CurrentSkill.SkillType)
        {
            case SkillType.Melee:
                // Handle melee skill
                if (evt == "HitboxOn")
                {
                    // Enable hitbox
                    Brain.EnableHitbox();
                }
                else if (evt == "HitboxOff")
                {
                    // Disable hitbox
                    Brain.DisableHitbox();
                }
                break;
            case SkillType.Projectile:
                // Handle projectile skill
                if (evt == "Spawn")
                {
                    Vector3 spawnPos =
                        Brain.transform.position +
                        Brain.transform.forward * 1.5f +
                        Vector3.up * 1.4f;

                    var projectile = GameObject.Instantiate(
                        Context.projectileSkill,
                        spawnPos,
                        Brain.transform.rotation);
                    projectile.GetComponent<Projectile>().Init(
                        Brain.gameObject,
                        Brain.transform.forward,
                        Context.projectileDamage);
                }
                break;
            case SkillType.AOE:
                // Handle AOE skill
                if (evt == "Spawn")
                {
                    // Spawn AOE effect
                    var aoe = GameObject.Instantiate(Context.aoeSkill, Brain.transform.position, Quaternion.identity);
                    aoe.GetComponent<GroundMeleeSkill>().Init(
                        Brain.gameObject,
                        Context.aoeDamage);
                    aoe.gameObject.SetActive(true);
                }
                break;
        }
    }

    protected override HierarchicalState GetTransition()
    {
        if (!finished)
            return null;

        var offensive =
            Parent as LK_OffensiveState;

        offensive.SelectSkill();

        return offensive.approachState;
    }
}