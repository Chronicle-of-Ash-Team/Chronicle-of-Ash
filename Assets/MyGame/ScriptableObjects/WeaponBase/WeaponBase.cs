using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public WeaponType weaponType;
    public Sprite icon;

    [Header("Stats")]
    public float damage = 1;
    public float attackSpeed = 1;
    public float staminaCost = 1;

    [Header("Animation")]
    public RuntimeAnimatorController animatorOverride;
    public float speed = 1;

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

