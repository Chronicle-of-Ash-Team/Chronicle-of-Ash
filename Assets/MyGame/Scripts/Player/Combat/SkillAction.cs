public class SkillAction : BaseCombatAction
{
    private void Start()
    {
        playerAnimation.OnAttackStart += PlayerAnimation_OnAttackStart;
    }

    private void PlayerAnimation_OnAttackStart()
    {
        IsRunning = true;
    }
    protected override void Execute()
    {
        playerLocomotion.StopMove();
        playerAnimation.PlaySkill();
    }

    public override void OnFinish()
    {
        playerLocomotion.ResumeMove();
        IsRunning = false;
    }
}
