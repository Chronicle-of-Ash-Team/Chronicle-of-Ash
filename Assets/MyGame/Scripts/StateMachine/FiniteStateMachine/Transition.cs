using UnityEngine;

public class Transition : ITransition
{
    public IFiniteState To { get; }

    public IPredicate Codition { get; }

    public Transition(IFiniteState to, IPredicate condition)
    {
        To = to;
        Codition = condition;
    }
}
