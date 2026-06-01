using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private float hitKnockbackForce = 8f;
    [SerializeField] private float parryKnockbackForce = 12f;
    [SerializeField] private float deathKnockbackForce = 5f;

    [SerializeField] private float maxKnockbackVelocity = 10f;

    public bool IsHit { get; private set; }

    private bool isAlive = true;

    public bool isInvincible = false;

    private BlockAction blockAction;
    private PlayerAnimation playerAnimation;
    private Rigidbody rb;
    private PlayerEvents playerEvents;


    private Vector3 knockbackVelocity;
    private float knockbackTimer;


    public event Action OnHit;
    public event Action OnHitEnd;
    public event Action OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        blockAction = GetComponent<BlockAction>();
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerEvents = GetComponent<PlayerEvents>();
    }

    void FixedUpdate()
    {
        if (knockbackTimer > 0)
        {
            rb.linearVelocity = new Vector3(
                knockbackVelocity.x,
                rb.linearVelocity.y,
                knockbackVelocity.z
            );

            knockbackTimer -= Time.fixedDeltaTime;
        }
    }

    public void TakeDamage(DamageContext damageContext)
    {
        Debug.Log("TakeDamage: " + damageContext.Damage);

        Vector3 pushDir = damageContext.HitDirection.normalized;
        pushDir.y = 0f;

        if (blockAction.isParrying)
        {
            blockAction.OnFinish();

            ApplyKnockbackVelocity(pushDir * parryKnockbackForce);

            int parryNum = UnityEngine.Random.Range(1, 4);
            playerAnimation.PlayThisAnimation("Parry" + parryNum, 0f);

            playerEvents.Parry_Event.Raise(damageContext);
            return;
        }

        if (isInvincible)
        {
            Debug.Log("You just dodge");
            return;
        }

        if (!blockAction.isBlocking)
        {
            ApplyKnockbackVelocity(pushDir * hitKnockbackForce);
        }

        currentHealth -= damageContext.Damage;

        playerEvents.Hit_Event.Raise(damageContext);

        playerEvents.HPChanged_Event.Raise(new HPContext
        {
            CurrentHP = currentHealth,
            MaxHP = maxHealth
        });

        if (currentHealth <= 0)
        {
            isAlive = false;

            // giảm velocity trước khi chết
            rb.linearVelocity = Vector3.zero;

            ApplyKnockbackVelocity(pushDir * deathKnockbackForce);

            OnDied?.Invoke();
            playerAnimation.PlayThisAnimation("Die", 0f);
            return;
        }

        OnHit?.Invoke();
    }

    public void ApplyKnockbackVelocity(Vector3 dir)
    {
        knockbackVelocity = dir;
        knockbackTimer = 0.15f;
    }

    public void EndHit()
    {
        //IsHit = false;
        OnHitEnd?.Invoke();
    }

    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }
}
