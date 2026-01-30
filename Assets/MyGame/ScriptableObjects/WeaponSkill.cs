using UnityEngine;

public abstract class WeaponSkill : ScriptableObject
{
    public string skillName;
    public float cooldown;
    public float staminaCost;
    public GameObject castEffect;
    public GameObject hitEffect;

    public abstract void Execute(SkillContext context);
}

public class SkillContext
{
    public Transform caster;
    public Transform target;
    public float damage;
}
