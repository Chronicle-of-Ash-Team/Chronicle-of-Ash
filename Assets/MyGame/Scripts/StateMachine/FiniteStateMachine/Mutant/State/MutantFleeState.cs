using UnityEngine;

public class MutantFleeState : BaseMutantState
{
    public MutantFleeState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("FLEE");
    }
}
