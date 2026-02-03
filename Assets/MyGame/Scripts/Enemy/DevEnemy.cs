using UnityEngine;

public class DevEnemy : MonoBehaviour, ILockable, IDamageable
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int currentHealth = 20;

    [SerializeField] private Material faceMaterial;
    [SerializeField] private Face faces;
    [SerializeField] private Transform lockOnPos;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Transform GetLockOnTransform()
    {
        return lockOnPos;
    }

    public void TakeDamage(int damage)
    {
        animator.CrossFade("Damage0", 0f);
        faceMaterial.mainTexture = faces.damageFace;
        currentHealth -= damage;
    }

    private void EndHit()
    {
        faceMaterial.mainTexture = faces.Idleface;
    }
}
