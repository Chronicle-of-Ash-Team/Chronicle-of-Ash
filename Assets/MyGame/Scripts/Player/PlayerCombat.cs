using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public BaseCombatAction CurrentAction { get; private set; }

    private AttackAction attackAction;
    private DodgeAction dodgeAction;
    private SkillAction skillAction;

    private PlayerAnimation playerAnimation;

    private void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();

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
        if (CurrentAction != null && CurrentAction.IsRunning)
            return;
        if (CurrentAction == action)
            return;
        action.OnFinish();
        action.TryExecute();
        CurrentAction = action;
    }

    public void OnActionFinished()
    {
        CurrentAction = null;
    }

    private void PlayerAnimation_OnDodgeEnd()
    {
        dodgeAction.OnFinish();
        OnActionFinished();
    }

    private void PlayerAnimation_OnAttackEnd()
    {
        attackAction.OnFinish();
        skillAction.OnFinish();
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
