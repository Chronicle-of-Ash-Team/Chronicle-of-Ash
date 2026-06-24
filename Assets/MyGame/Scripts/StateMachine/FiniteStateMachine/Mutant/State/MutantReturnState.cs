using UnityEngine;

public class MutantReturnState : BaseMutantState
{
    public MutantReturnState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("RETURN");
    }
}
