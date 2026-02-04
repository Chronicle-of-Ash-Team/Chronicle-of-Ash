using UnityEngine;

public class DevEnemy : MonoBehaviour, ILockable, IDamageable
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int currentHealth;

    [SerializeField] private Material faceMaterial;
    [SerializeField] private Face faces;
    [SerializeField] private Transform lockOnPos;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public Transform GetLockOnTransform()
    {
        return lockOnPos;
    }

    public void TakeDamage(DamageContext damageContext)
    {
        animator.CrossFade("Damage0", 0f);
        faceMaterial.mainTexture = faces.damageFace;
        currentHealth -= damageContext.Damage;
    }

    private void EndHit()
    {
        faceMaterial.mainTexture = faces.Idleface;
    }

    public void SetInvincible(bool invincible)
    {
        throw new System.NotImplementedException();
    }
}
