using UnityEngine;

public interface IDamageable
{
    void SetInvincible(bool invincible);
    void TakeDamage(int damage, GameObject attacker);
}
