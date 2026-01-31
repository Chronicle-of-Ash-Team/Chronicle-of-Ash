using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public WeaponType weaponType;
    public Sprite icon;

    [Header("Stats")]
    public float damage;
    public float attackSpeed;
    public float staminaCost;

    [Header("Animation")]
    public RuntimeAnimatorController animatorOverride;
    public float speed;

    [Header("Prefab")]
    public GameObject weaponPrefab;

    //[Header("Skill")]

    public abstract void Execute(WeaponSkillContext context);
}

public class WeaponSkillContext
{
    public Transform caster;
    public Transform target;
    public float damage;
}

