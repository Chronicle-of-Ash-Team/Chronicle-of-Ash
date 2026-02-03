public class HitAction : BaseCombatAction
{
    private void Start()
    {
        baseAnimation.OnHitEnd += PlayerAnimation_OnHitEnd;
    }

    private void PlayerAnimation_OnHitEnd()
    {
        OnFinish();
    }

    public override void TryExecute()
    {
        Execute();
    }

    public override void OnFinish()
    {
        IsRunning = false;
        locomotion.ResumeMove();
        actionHandler.OnActionFinished(this);
    }

    protected override void Execute()
    {
        IsRunning = true;
        locomotion.StopMove();
        baseAnimation.PlayThisAnimationWithUpper("Hit");
    }
}
