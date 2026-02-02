using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;

    public bool IsHit = false;

    private PlayerAnimation playerAnimation;
    private PlayerLocomotion playerLocomotion;
    private PlayerCombat playerCombat;

    private DodgeAction dodgeAction;

    private void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerCombat = GetComponent<PlayerCombat>();
        dodgeAction = GetComponent<DodgeAction>();


        playerAnimation.OnHitStart += PlayerAnimation_OnHitStart;
        playerAnimation.OnHitEnd += PlayerAnimation_OnHitEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!dodgeAction.IsRunning)
        {
            playerCombat.EndAllAction();
            playerAnimation.PlayHit();
            playerLocomotion.StopMove();

            currentHealth -= damage;

            if (currentHealth < 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {

    }

    private void PlayerAnimation_OnHitStart()
    {
        playerLocomotion.StopMove();
        IsHit = true;
    }

    private void PlayerAnimation_OnHitEnd()
    {
        playerLocomotion.ResumeMove();
        IsHit = false;
    }
}
