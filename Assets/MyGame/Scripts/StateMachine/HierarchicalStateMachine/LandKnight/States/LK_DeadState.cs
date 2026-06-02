public class LK_DeadState : LK_BaseState
{
    public LK_DeadState(HierarchicalStateMachine machine, HierarchicalState parent, LandKnightBrain brain, LandKnightContext context) : base(machine, parent, brain, context)
    {
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        Context.animator.Play("Die");
    }
}
