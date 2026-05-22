using UnityEngine;

public class MutantDeadState : BaseMutantState
{
    public MutantDeadState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("DEAD");

        animator.CrossFade(DeathHash, CrossFadeDuration);
    }
}
