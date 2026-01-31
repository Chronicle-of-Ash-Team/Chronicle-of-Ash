using UnityEngine;

[CreateAssetMenu(fileName = "MeleeSkillWeapon", menuName = "Game/Weapon/MeleeSkillWeapon")]
public class MeleeSkillWeapon : WeaponBase
{
    [Header("Skill")]
    public GameObject holdEffect;
    public GameObject castEffect;
    public override void Execute(WeaponSkillContext context)
    {
        throw new System.NotImplementedException();
    }
}
