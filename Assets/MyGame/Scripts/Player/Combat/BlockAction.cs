public class BlockAction : BaseCombatAction
{
    private bool isHolding;
    public bool isBlocking;
    public bool isParrying;

    private void Start()
    {
        baseAnimation.OnBlockStart += BaseAnimation_OnBlockStart;

        baseAnimation.OnActionEventStart += BaseAnimation_OnActionEventStart;
        baseAnimation.OnActionEventEnd += BaseAnimation_OnActionEventEnd;

        baseAnimation.OnParryStart += BaseAnimation_OnParryStart;
        baseAnimation.OnParryEnd += BaseAnimation_OnParryEnd;
    }

    private void BaseAnimation_OnParryEnd()
    {
        isParrying = false;
    }

    private void BaseAnimation_OnParryStart()
    {
        isParrying = true;
    }

    private void BaseAnimation_OnActionEventEnd()
    {
        if (!IsThisAction) return;
        if (isHolding)
        {
            IsRunning = true;
        }
        else
        {
            ShutdownBlock();
        }
    }

    private void BaseAnimation_OnActionEventStart()
    {
        if (!IsThisAction) return;
        IsRunning = true;
    }

    private void BaseAnimation_OnBlockStart()
    {
        isBlocking = true;

    }

    public override void OnFinish()
    {
        ShutdownBlock();
    }

    public void OnBlockCancel()
    {
        isHolding = false;

        if (isBlocking)
        {
            ShutdownBlock();
        }
    }

    public void ShutdownBlock()
    {
        baseAnimation.SetBlocking(false);
        IsThisAction = false;
        IsRunning = false;
        isHolding = false;
        isBlocking = false;
        isParrying = false;
        locomotion.ResumeMove();
    }

    protected override void Execute()
    {
        isHolding = true;
        IsThisAction = true;
        IsRunning = true;

        baseAnimation.SetBlocking(false);
        baseAnimation.PlayThisAnimation("Block", 0f);
        baseAnimation.SetBlocking(true);
        locomotion.StopMove();
    }
}
