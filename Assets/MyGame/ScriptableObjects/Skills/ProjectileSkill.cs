using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSkill", menuName = "Game/Skill/ProjectileSkill")]
public class ProjectileSkill : WeaponSkill
{
    public GameObject projectile;
    public override void Execute(SkillContext context)
    {
        throw new System.NotImplementedException();
    }
}
