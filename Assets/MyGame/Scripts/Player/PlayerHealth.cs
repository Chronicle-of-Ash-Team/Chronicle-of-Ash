using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;

    public bool IsHit { get; private set; }
    public bool isInvincible = false;

    public event Action OnHit;
    public event Action OnHitEnd;
    public event Action OnDied;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

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
}
