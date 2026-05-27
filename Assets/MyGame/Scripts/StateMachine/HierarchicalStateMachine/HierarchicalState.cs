using System.Collections.Generic;

public class HierarchicalState
{
    public readonly HierarchicalStateMachine Machine;
    public readonly HierarchicalState Parent;
    public HierarchicalState ActiveChild;

    readonly List<IActivity> activities = new List<IActivity>();
    public IReadOnlyList<IActivity> Activities => activities;

    public HierarchicalState(HierarchicalStateMachine stateMachine, HierarchicalState parentState)
    {
        Machine = stateMachine;
        Parent = parentState;
    }

    protected virtual HierarchicalState GetInitialState() => null;
    protected virtual HierarchicalState GetTransition() => null;

    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }
    protected virtual void OnUpdate(float deltaTime) { }

    internal void Enter()
    {
        if (Parent != null) Parent.ActiveChild = this;

        OnEnter();

        HierarchicalState init = GetInitialState();

        if (init != null) init.Enter();
    }

    internal void Exit()
    {
        if (ActiveChild != null) ActiveChild.Exit();

        ActiveChild = null;

        OnExit();
    }

    internal void Update(float deltaTime)
    {
        HierarchicalState transition = GetTransition();
        if (transition != null)
        {
            Machine.Sequencer.RequestTransition(this, transition);
            return;
        }

        if (ActiveChild != null) ActiveChild.Update(deltaTime);

        OnUpdate(deltaTime);
    }

    public HierarchicalState Leaf()
    {
        HierarchicalState state = this;
        while (state.ActiveChild != null) state = state.ActiveChild;
        return state;
    }

    public IEnumerable<HierarchicalState> PathToRoot()
    {
        for (HierarchicalState state = this; state != null; state = state.Parent) yield return state;
    }
}
