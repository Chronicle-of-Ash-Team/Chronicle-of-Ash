using System.Collections.Generic;
using UnityEngine;

public class StateNode
{
    public IFiniteState State { get; }

    public HashSet<ITransition> Transitions { get; }

    public StateNode(IFiniteState state)
    {
        State = state;
        Transitions = new HashSet<ITransition>();
    }

    public void AddTransition(IFiniteState to, IPredicate condition)
    {
        Transitions.Add(new Transition(to, condition));
    }
}
