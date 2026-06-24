public class LK_CombatState : LK_BaseState
{
    public LK_OffensiveState offensiveState;
    public LK_DefensiveState defensiveState;

    public LK_CombatState(HierarchicalStateMachine machine, HierarchicalState parent, LandKnightBrain brain, LandKnightContext context) : base(machine, parent, brain, context)
    {
        offensiveState = new LK_OffensiveState(machine, this, brain, context);
        defensiveState = new LK_DefensiveState(machine, this, brain, context);
    }

    protected override HierarchicalState GetInitialState()
    {
        //if (Brain.IsHealthLow())
        //{
        //    return defensiveState;
        //}

        return offensiveState;
    }

    protected override HierarchicalState GetTransition()
    {
        if (!Brain.IsPlayerInRange(
            Context.loseAggroRange))
        {
            var root =
                Parent as LK_RootState;

            return root.patrolState;
        }

        return null;
    }
}

public class SkillData
{
    public string AnimationName;
    public float MinRange;
    public float MaxRange;
    public SkillType SkillType;
}

public enum SkillType
{
    Melee,
    Projectile,
    AOE
}
