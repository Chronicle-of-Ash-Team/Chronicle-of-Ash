public class BlockAction : BaseCombatAction
{
    private bool isHolding;
    public bool isBlocking;
    public bool isParrying;
    private bool animReachedEnd;

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

        animReachedEnd = true;

        if (!isHolding)
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
        baseAnimation.SetBlocking(true);
    }

    public override void OnFinish()
    {
        ShutdownBlock();
    }

    public void OnBlockCancel()
    {
        isHolding = false;

        if (!animReachedEnd) return;

        ShutdownBlock();
    }

    public void ShutdownBlock()
    {
        IsRunning = false;
        isHolding = false;
        isBlocking = false;
        isParrying = false;
        IsThisAction = false;
        animReachedEnd = false;
        baseAnimation.SetBlocking(false);
        locomotion.ResumeMove();
    }

    protected override void Execute()
    {
        IsRunning = true;
        isHolding = true;
        IsThisAction = true;

        baseAnimation.SetBlocking(false);
        baseAnimation.PlayThisAnimation("Block", 0f);
        locomotion.StopMove();
    }
}
