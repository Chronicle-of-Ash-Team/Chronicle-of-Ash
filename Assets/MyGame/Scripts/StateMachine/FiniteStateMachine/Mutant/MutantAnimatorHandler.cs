using UnityEngine;

public class MutantAnimatorHandler : MonoBehaviour
{
    [SerializeField] private MutantBrain brain;
    [SerializeField] private WeaponHitBox[] weaponHitBoxes;

    void Start()
    {
        foreach (var hitBox in weaponHitBoxes)
        {
            hitBox.Init(brain);
        }
    }

    public void StartAttack()
    {
        brain.isAttacking = true;
        foreach (var hitBox in weaponHitBoxes)
        {
            hitBox.EnableHitbox();
        }
    }
    public void EndAttack()
    {
        brain.isAttacking = false;
        foreach (var hitBox in weaponHitBoxes)
        {
            hitBox.DisableHitbox();
        }
    }
}
