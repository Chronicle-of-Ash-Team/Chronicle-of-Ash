public interface IDamageable
{
    bool GetIsAlive();
    void SetInvincible(bool invincible);
    void TakeDamage(DamageContext damageContext);
}
