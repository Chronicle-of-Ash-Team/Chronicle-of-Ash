using UnityEngine;

public class MutantStaggerState : BaseMutantState
{
    public MutantStaggerState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("STAGGER");

        animator.CrossFade(HitHash, CrossFadeDuration);
    }
}
