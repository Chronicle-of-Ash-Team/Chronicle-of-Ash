using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public BaseCombatAction CurrentAction { get; private set; }

    private AttackAction attackAction;
    private DodgeAction dodgeAction;
    private SkillAction skillAction;

    private PlayerAnimation playerAnimation;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerHealth = GetComponent<PlayerHealth>();

        attackAction = GetComponent<AttackAction>();
        dodgeAction = GetComponent<DodgeAction>();
        skillAction = GetComponent<SkillAction>();

        GameInput.Instance.OnDodgePerformed += GameInput_OnDodgePerformed;
        GameInput.Instance.OnAttackPerformed += GameInput_OnAttackPerformed;
        GameInput.Instance.OnSkillPerformed += GameInput_OnSkillPerformed;

        playerAnimation.OnAttackEnd += PlayerAnimation_OnAttackEnd;
        playerAnimation.OnDodgeEnd += PlayerAnimation_OnDodgeEnd;
    }

    public void TryAction(BaseCombatAction action)
    {
        if (playerHealth.IsHit) return;
        if (CurrentAction != null && CurrentAction.IsRunning) return;
        if (CurrentAction == action) return;
        action.OnFinish();
        action.TryExecute();
        CurrentAction = action;
    }

    public void OnActionFinished()
    {
        CurrentAction = null;
    }

    public void EndAllAction()
    {
        dodgeAction.OnFinish();
        skillAction.OnFinish();
        attackAction.OnFinish();
        CurrentAction = null;
    }

    private void PlayerAnimation_OnDodgeEnd()
    {
        EndAllAction();
        OnActionFinished();
    }

    private void PlayerAnimation_OnAttackEnd()
    {
        EndAllAction();
        OnActionFinished();
    }

    private void GameInput_OnAttackPerformed()
    {
        TryAction(attackAction);
    }

    private void GameInput_OnSkillPerformed()
    {
        TryAction(skillAction);
    }

    private void GameInput_OnDodgePerformed()
    {
        TryAction(dodgeAction);
    }
}
