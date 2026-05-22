using UnityEngine;

public class MutantAttackState : BaseMutantState
{
    public MutantAttackState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ATTACK");
        animator.CrossFade(Attack1Hash, CrossFadeDuration);
    }

    public override void OnExit()
    {
        base.OnExit();
        brain.mutantAnimatorHandler.EndAttack();
    }
}
