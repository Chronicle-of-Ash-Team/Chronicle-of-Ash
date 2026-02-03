using UnityEngine;

public class AttackAction : BaseCombatAction
{
    [Header("Attack Settings")]
    [SerializeField] private float comboResetTime = 1.2f;
    [SerializeField] int attackComboCount = 1;

    private float comboTimer;

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

    private void Update()
    {
        HandleComboTimer();
    }

    private void HandleComboTimer()
    {
        if (attackComboCount == 0) return;

        comboTimer += Time.deltaTime;

        if (comboTimer >= comboResetTime)
        {
            attackComboCount = 0;
            comboTimer = 0f;
        }
    }

    protected override void Execute()
    {
        locomotion.StopMove();
        comboTimer = 0f;

        attackComboCount++;

        if (attackComboCount > 3)
        {
            attackComboCount = 1;
        }
        baseAnimation.PlayThisAnimation("Attack" + attackComboCount.ToString());
    }

    public override void OnFinish()
    {
        locomotion.ResumeMove();
        IsRunning = false;
        actionHandler.OnActionFinished(this);
    }
}
