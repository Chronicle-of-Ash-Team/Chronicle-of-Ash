using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public bool IsHit { get; private set; }
    public bool isInvincible = false;

    public GameObject parryParticle;

    private BlockAction blockAction;
    private PlayerAnimation playerAnimation;
    private Rigidbody rb;

    public event Action OnHit;
    public event Action OnHitEnd;
    public event Action OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        blockAction = GetComponent<BlockAction>();
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            //TakeDamage(new DamageContext
            //{
            //    Attacker = gameObject,
            //    Damage = 1,
            //    DamageType = DamageType.Normal,
            //    HitDirection = Vector3.forward,
            //});
        }
    }

    public void TakeDamage(DamageContext damageContext)
    {
        if (blockAction.isParrying)
        {
            playerAnimation.PlayThisAnimation("Parry", 0.3f);
            blockAction.ShutdownBlock();
            rb.AddForce(damageContext.HitDirection * 25f, ForceMode.Impulse);
            Destroy(Instantiate(parryParticle, damageContext.HitPosition, Quaternion.identity), 1f);
            return;
        }
        if (isInvincible)
        {
            Debug.Log("You just dodge");
            return;
        }
        rb.AddForce(damageContext.HitDirection * 45f, ForceMode.Impulse);

        if (blockAction.isBlocking)
        {
            Debug.Log("IsBlocking");
            return;
        }


        currentHealth -= damageContext.Damage;

        if (currentHealth <= 0)
        {
            OnDied?.Invoke();
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
}
