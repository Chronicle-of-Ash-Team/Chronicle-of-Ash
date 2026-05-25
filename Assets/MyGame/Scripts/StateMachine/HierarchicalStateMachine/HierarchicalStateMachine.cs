using System.Collections.Generic;

public class HierarchicalStateMachine
{
    public readonly HierarchicalState Root;
    public readonly TransititionSequencer Sequencer;

    bool started = false;

    public HierarchicalStateMachine(HierarchicalState rootState)
    {
        Root = rootState;
        Sequencer = new TransititionSequencer(this);
    }

    public void Start()
    {
        if (started) return;
        started = true;
        Root.Enter();
    }

    public void Tick(float deltaTime)
    {
        if (!started) Start();

        InternalTick(deltaTime);
    }

    internal void InternalTick(float deltaTime)
    {
        Root.Update(deltaTime);
    }


    public void ChangeState(HierarchicalState from, HierarchicalState to)
    {
        if (from == to || from == null || to == null) return;

        HierarchicalState lca = TransititionSequencer.Lca(from, to);

        for (HierarchicalState state = from; state != lca; state = state.Parent)
        {
            state.Exit();
        }

        var stack = new Stack<HierarchicalState>();
        for (HierarchicalState state = to; state != lca; state = state.Parent)
        {
            stack.Push(state);
        }

        while (stack.Count > 0)
        {
            stack.Pop().Enter();
        }
    }
}
