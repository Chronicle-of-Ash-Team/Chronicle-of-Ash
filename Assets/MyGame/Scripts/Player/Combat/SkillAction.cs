public class SkillAction : BaseCombatAction
{
    private void Start()
    {
        playerAnimation.OnAttackStart += PlayerAnimation_OnAttackStart;
        playerAnimation.OnAttackEnd += PlayerAnimation_OnAttackEnd;
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
        playerAnimation.PlaySkill();
    }

    public override void OnFinish()
    {
        locomotion.ResumeMove();
        IsRunning = false;
        actionHandler.OnActionFinished(this);
    }
}
