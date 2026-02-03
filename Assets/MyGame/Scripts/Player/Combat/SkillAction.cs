public class SkillAction : BaseCombatAction
{
    private void Start()
    {
        baseAnimation.OnActionEventStart += BaseAnimation_OnActionEventStart;
        baseAnimation.OnActionEventEnd += BaseAnimation_OnActionEventEnd;
    }

    private void BaseAnimation_OnActionEventEnd()
    {
        if (!IsThisAction) return;
        OnFinish();
    }

    private void BaseAnimation_OnActionEventStart()
    {
        if (!IsThisAction) return;
        IsRunning = true;
    }
    protected override void Execute()
    {
        IsThisAction = true;

        locomotion.StopMove();
        baseAnimation.PlayThisAnimation("Skill");
    }

    public override void OnFinish()
    {
        IsThisAction = false;

        locomotion.ResumeMove();
        IsRunning = false;
        actionHandler.OnActionFinished(this);
    }
}
