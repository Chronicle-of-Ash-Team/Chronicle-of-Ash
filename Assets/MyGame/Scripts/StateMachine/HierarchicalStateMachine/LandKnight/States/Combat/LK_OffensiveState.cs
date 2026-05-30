using UnityEngine;



public class LK_OffensiveState : LK_BaseState
{
    public LK_ApproachState approachState;
    public LK_AttackState attackState;
    public LK_SkillState skillState;

    public SkillData CurrentSkill;


    public SkillData[] skills = new SkillData[]
    {
        new SkillData
        {
            AnimationName = "SkillMelee",
            MinRange = 1f,
            MaxRange = 2f,
            SkillType = SkillType.Melee
        },
        new SkillData
        {
            AnimationName = "SkillMelee2",
            MinRange = 1f,
            MaxRange = 2f,
            SkillType = SkillType.Melee
        },
        new SkillData
        {
            AnimationName = "SkillProjectile",
            MinRange = 5f,
            MaxRange = 10f,
            SkillType = SkillType.Projectile
        },
        new SkillData
        {
            AnimationName = "SkillAOE",
            MinRange = 0f,
            MaxRange = 3f,
            SkillType = SkillType.AOE
        }
    };

    public LK_OffensiveState(HierarchicalStateMachine machine, HierarchicalState parent, LandKnightBrain brain, LandKnightContext context) : base(machine, parent, brain, context)
    {
        approachState = new LK_ApproachState(machine, this, brain, context);
        attackState = new LK_AttackState(machine, this, brain, context);
        skillState = new LK_SkillState(machine, this, brain, context);

        SelectSkill();
    }

    protected override HierarchicalState GetInitialState()
    {
        return approachState;
    }

    protected override HierarchicalState GetTransition()
    {
        return base.GetTransition();
    }
    public void SelectSkill()
    {
        CurrentSkill =
            skills[Random.Range(0, skills.Length)];
    }
}
