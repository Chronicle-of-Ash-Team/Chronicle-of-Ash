using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : BaseAnimation
{
    private void StartRoll()
    {
        OnDodgeStartEvent();
    }
    private void EndRoll()
    {
        BlendUpper();
        OnDodgeEndEvent();
    }
    private void StartAttack()
    {
        animator.SetLayerWeight(1, 0f);
        OnAttackStartEvent();
    }
    private void EndAttack()
    {
        BlendUpper();
        OnAttackEndEvent();
    }
    private void StartHit()
    {
        OnHitStartEvent();
    }
    private void EndHit()
    {
        BlendUpper();
        OnHitEndEvent();
    }
    private void StartBlock()
    {
        OnBlockStartEvent();
    }
    private void EndBlock()
    {
        OnBlockEndEvent();
    }
    private void StartParry()
    {
        OnParryStartEvent();
    }
    private void EndParry()
    {
        OnParryEndEvent();
    }
    private void StartAction()
    {
        OnActionEventStartEvent();
    }
    private void EndAction()
    {
        OnActionEventEndEvent();
    }
}
