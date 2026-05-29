public abstract class LK_BaseState : HierarchicalState
{
    protected readonly LandKnightBrain Brain;
    protected readonly LandKnightContext Context;

    protected LK_BaseState(
        HierarchicalStateMachine machine,
        HierarchicalState parent,
        LandKnightBrain brain,
        LandKnightContext context)
        : base(machine, parent)
    {
        Brain = brain;
        Context = context;
    }
}