using UnityEngine;

public class BlockAction : BaseCombatAction
{
    private void Start()
    {
        baseAnimation.OnBlockStart += BaseAnimation_OnBlockStart;

        baseAnimation.OnActionEventStart += BaseAnimation_OnActionEventStart;
        baseAnimation.OnActionEventEnd += BaseAnimation_OnActionEventEnd;
    }

    private void BaseAnimation_OnActionEventEnd()
    {
        IsRunning = false;
    }

    private void BaseAnimation_OnActionEventStart()
    {
        IsRunning = true;
    }

    private void BaseAnimation_OnBlockStart()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnFinish()
    {
        IsThisAction = false;
        IsRunning = false;

        baseAnimation.SetBlocking(false);
        locomotion.ResumeMove();
    }

    protected override void Execute()
    {
        IsThisAction = true;

        baseAnimation.PlayThisAnimation("Block", 0f);
        baseAnimation.SetBlocking(true);
        locomotion.StopMove();
    }
}
