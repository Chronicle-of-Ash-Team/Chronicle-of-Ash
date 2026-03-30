using UnityEngine;

[CreateAssetMenu(fileName = "MeleeSkillWeapon", menuName = "Game/Weapon/ProjectileSkillWeapon")]
public class ProjectileSkillWeapon : WeaponBase
{
    [Header("Skill")]
    [SerializeField] private int projectileDamage = 1;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectileCount = 3;
    [SerializeField] private float radius = 1.5f;

    public override void Execute(WeaponSkillContext context)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab is null!");
            return;
        }

        Vector3 center = context.caster.position;
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            Vector3 spawnPos = center + offset + Vector3.up * 1.5f;

            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            Vector3 direction;
            if (context.target == null)
            {
                direction = context.caster.forward;
            }
            else
            {
                direction = (context.target.position - projectile.transform.position).normalized;
            }


            if (projectile.TryGetComponent(out Projectile proj))
            {
                proj.Init(context.caster.gameObject, direction, projectileDamage);
            }
        }
    }
}
