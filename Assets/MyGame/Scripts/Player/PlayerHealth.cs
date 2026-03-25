using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public bool IsHit { get; private set; }

    private bool isAlive = true;

    public bool isInvincible = false;

    public GameObject parryParticle;

    private BlockAction blockAction;
    private PlayerAnimation playerAnimation;
    private Rigidbody rb;
    private PlayerEvents playerEvents;

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

    public void TakeDamage(DamageContext damageContext)
    {
        Vector3 pushDir = damageContext.HitDirection;
        pushDir.y = 0f;

        if (blockAction.isParrying)
        {
            //Parry success
            blockAction.ShutdownBlock();
            rb.AddForce(pushDir * 25f, ForceMode.Impulse);
            int parryNum = UnityEngine.Random.Range(1, 4);
            playerAnimation.PlayThisAnimation("Parry" + parryNum, 0.1f);
            Destroy(Instantiate(parryParticle, damageContext.HitPosition, Quaternion.identity), 1f);
            return;
        }
        if (isInvincible)
        {
            Debug.Log("You just dodge");
            return;
        }
        rb.AddForce(pushDir * 45f, ForceMode.Impulse);

        if (blockAction.isBlocking)
        {
            Debug.Log("IsBlocking");
            return;
        }


        currentHealth -= damageContext.Damage;
        playerEvents.HPChanged_Event.Raise(new HPContext
        {
            CurrentHP = currentHealth,
            MaxHP = maxHealth
        });

        if (currentHealth <= 0)
        {
            isAlive = false;
            OnDied?.Invoke();
            playerAnimation.PlayThisAnimation("Die", 0f);
            return;
        }
        OnHit?.Invoke();
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
