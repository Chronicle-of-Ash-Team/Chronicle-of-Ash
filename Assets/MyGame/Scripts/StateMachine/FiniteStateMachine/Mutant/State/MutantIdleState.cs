using UnityEngine;

public class MutantIdleState : BaseMutantState
{
    public MutantIdleState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("IDLE");

        animator.CrossFade(IdleHash, CrossFadeDuration);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        brain.stamina += Time.deltaTime * 5f;
    }
}
