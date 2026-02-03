public class SkillAction : BaseCombatAction
{
    private void Start()
    {
        baseAnimation.OnAttackStart += PlayerAnimation_OnAttackStart;
        baseAnimation.OnAttackEnd += PlayerAnimation_OnAttackEnd;
    }

    private void PlayerAnimation_OnAttackEnd()
    {
        OnFinish();
    }

    private void PlayerAnimation_OnAttackStart()
    {
        IsRunning = true;
    }
    protected override void Execute()
    {
        locomotion.StopMove();
        baseAnimation.PlayThisAnimation("Skill");
    }

    public override void OnFinish()
    {
        locomotion.ResumeMove();
        IsRunning = false;
        actionHandler.OnActionFinished(this);
    }
}
