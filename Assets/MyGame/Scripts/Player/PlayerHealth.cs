using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public bool IsHit { get; private set; }
    public bool isInvincible = false;

    public event Action OnHit;
    public event Action OnHitEnd;
    public event Action OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(1, gameObject);
        }
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        if (isInvincible)
        {
            Debug.Log("You just dodge");
            return;
        }

        currentHealth -= damage;

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
