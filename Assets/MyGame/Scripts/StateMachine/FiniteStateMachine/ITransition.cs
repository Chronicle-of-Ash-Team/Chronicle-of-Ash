using UnityEngine;

public interface ITransition
{
    IFiniteState To { get; }
    IPredicate Codition { get; }
}
