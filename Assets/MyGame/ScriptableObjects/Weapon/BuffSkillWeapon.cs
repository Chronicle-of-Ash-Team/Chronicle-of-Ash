using UnityEngine;

[CreateAssetMenu(fileName = "BuffSkillWeapon", menuName = "Game/Weapon/BuffSkillWeapon")]
public class BuffSkillWeapon : WeaponBase
{
    [Header("Skill")]
    public GameObject buffEffect;
    public override void Execute(WeaponSkillContext context)
    {
        throw new System.NotImplementedException();
    }
}
