public class LK_RootState : LK_BaseState
{
    public LK_DeadState deadState;
    public LK_PatrolState patrolState;
    public LK_CombatState combatState;
    public LK_StaggerState staggerState;

    public LK_RootState(HierarchicalStateMachine machine, HierarchicalState parent, LandKnightBrain brain, LandKnightContext context) : base(machine, parent, brain, context)
    {
        deadState = new LK_DeadState(machine, this, brain, context);
        patrolState = new LK_PatrolState(machine, this, brain, context);
        combatState = new LK_CombatState(machine, this, brain, context);
        staggerState = new LK_StaggerState(machine, this, brain, context);
    }

    override protected HierarchicalState GetInitialState()
    {
        return patrolState;
    }

    protected override HierarchicalState GetTransition()
    {
        var leaf = Machine.Root.Leaf();

        // dead priority cao nhất
        if (Context.currentHealth <= 0f)
        {
            if (leaf != deadState)
            {
                return deadState;
            }
        }

        // posture break
        if (Context.postureBroken)
        {
            if (leaf != staggerState)
            {
                return staggerState;
            }
        }

        return null;
    }
}
