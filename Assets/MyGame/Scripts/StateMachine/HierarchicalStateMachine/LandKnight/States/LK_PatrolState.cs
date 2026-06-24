public class LK_PatrolState : LK_BaseState
{
    public LK_MoveToPointState moveToPointState;
    public LK_RestState restState;
    public LK_WaitState waitState;

    public LK_PatrolState(
        HierarchicalStateMachine machine,
        HierarchicalState parent,
        LandKnightBrain brain,
        LandKnightContext context)
        : base(machine, parent, brain, context)
    {
        moveToPointState =
            new LK_MoveToPointState(
                machine,
                this,
                brain,
                context);

        restState =
            new LK_RestState(
                machine,
                this,
                brain,
                context);
        waitState =
            new LK_WaitState(
                machine,
                this,
                brain,
                context);
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        Context.currentSpeed = Context.moveSpeed;
    }

    protected override HierarchicalState GetInitialState()
    {
        return moveToPointState;
    }

    protected override HierarchicalState GetTransition()
    {
        if (Brain.IsPlayerInRange(
            Context.detectionRange))
        {
            var root =
                Parent as LK_RootState;

            return root.combatState;
        }

        return null;
    }
}