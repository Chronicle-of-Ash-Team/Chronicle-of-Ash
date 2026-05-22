using UnityEngine;

public class MutantStaggerState : BaseMutantState
{
    private float timer;

    public MutantStaggerState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("STAGGER");

        timer = brain.staggerDuration;

        animator.CrossFade(HitHash, CrossFadeDuration);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            brain.ClearHit();
        }
    }
}
