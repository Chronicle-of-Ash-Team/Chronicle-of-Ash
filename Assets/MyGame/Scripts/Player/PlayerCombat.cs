using UnityEngine;

public class PlayerCombat : MonoBehaviour, IActionHandler
{
    public BaseCombatAction CurrentAction { get; private set; }

    private AttackAction attackAction;
    private DodgeAction dodgeAction;
    private SkillAction skillAction;
    private HitAction hitAction;
    private BlockAction blockAction;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        attackAction = GetComponent<AttackAction>();
        dodgeAction = GetComponent<DodgeAction>();
        skillAction = GetComponent<SkillAction>();
        hitAction = GetComponent<HitAction>();
        blockAction = GetComponent<BlockAction>();

        GameInput.Instance.OnDodgePerformed += GameInput_OnDodgePerformed;
        GameInput.Instance.OnAttackPerformed += GameInput_OnAttackPerformed;
        GameInput.Instance.OnSkillPerformed += GameInput_OnSkillPerformed;
        GameInput.Instance.OnBlockPerformed += GameInput_OnBlockPerformed;

        playerHealth.OnHit += PlayerHealth_OnHit;
    }

    public void TryAction(BaseCombatAction action)
    {
        if (action == hitAction)
        {
            if (CurrentAction != null && CurrentAction != hitAction)
            {
                CurrentAction.OnFinish();
                CurrentAction = null;
            }

            action.TryExecute();
            CurrentAction = action;

            return;
        }

        if (CurrentAction != null && CurrentAction.IsRunning)
            return;

        if (CurrentAction == action)
            return;

        // Execute action mới
        action.OnFinish();
        CurrentAction?.OnFinish();
        action.TryExecute();
        CurrentAction = action;
    }

    public void EndAllAction()
    {
        CurrentAction?.OnFinish();
        CurrentAction = null;
    }

    public void OnActionFinished(BaseCombatAction action)
    {
        if (CurrentAction == action)
            CurrentAction = null;
    }

    private void PlayerHealth_OnHit()
    {
        TryAction(hitAction);
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
    private void GameInput_OnBlockPerformed(bool obj)
    {
        if (obj)
        {
            TryAction(blockAction);
        }
        else
        {
            blockAction.OnBlockCancel();
            OnActionFinished(blockAction);
        }
    }
}
