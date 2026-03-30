using UnityEngine;

public class PlayerWeapon : MonoBehaviour, IWeaponOwner
{
    [SerializeField] private Transform rightHandHolder;
    [SerializeField] private WeaponBase weaponData;
    [SerializeField] private float maxDistance = 5f;

    private WeaponController currentWeapon;
    private PlayerAnimation playerAnimation;

    private void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();

        playerAnimation.OnAttackStart += PlayerAnimation_OnAttackStart;
        playerAnimation.OnAttackEnd += PlayerAnimation_OnAttackEnd;
        playerAnimation.OnSkillStart += PlayerAnimation_OnSkillStart;

        ChangeWeapon(weaponData.weaponPrefab);
    }

    private void PlayerAnimation_OnSkillStart()
    {
        weaponData.Execute(new WeaponSkillContext
        {
            caster = transform,
            target = GetComponent<PlayerTargetLock>().currentTarget,
            damage = weaponData.damage
        });
    }

    private void PlayerAnimation_OnAttackEnd()
    {
        currentWeapon.GetWeaponHitBox().DisableHitbox();
    }

    private void PlayerAnimation_OnAttackStart()
    {
        currentWeapon.GetWeaponHitBox().EnableHitbox();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            HandleInteract();
        }
    }

    private void OnDestroy()
    {
        if (playerAnimation == null) return;

        playerAnimation.OnAttackStart -= PlayerAnimation_OnAttackStart;
        playerAnimation.OnAttackEnd -= PlayerAnimation_OnAttackEnd;
    }

    private void ChangeWeapon(GameObject newWeaponPref)
    {
        // 1. Gỡ vũ khí cũ
        if (currentWeapon != null)
        {
            currentWeapon.GetWeaponHitBox().DisableHitbox();
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
        }

        // 2. Spawn vũ khí mới
        GameObject weaponGO = Instantiate(newWeaponPref, rightHandHolder);
        weaponGO.transform.localPosition = Vector3.zero;
        weaponGO.transform.localRotation = Quaternion.identity;

        // 3. Cache weapon
        currentWeapon = weaponGO.GetComponent<WeaponController>();

        // 4. Init
        currentWeapon.Init(this);

        // 5. Apply animation theo weapon mới
        playerAnimation.ApplyWeapon(currentWeapon.GetWeaponData());
    }

    private void HandleInteract()
    {
        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
        {
            var weapon = hit.transform.GetComponentInParent<IWeaponProp>();
            if (weapon != null && Input.GetKeyDown(KeyCode.E))
            {
                ChangeWeapon(weapon.GetWeaponPref());
            }
        }
    }

    public int GetDamage()
    {
        return currentWeapon.GetWeaponData().damage;
    }

    public WeaponBase GetWeaponData()
    {
        return currentWeapon.GetWeaponData();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnWeaponHit(IDamageable target, Collider other, WeaponHitBox hitbox)
    {
        target.TakeDamage(new DamageContext
        {
            Attacker = gameObject,
            Damage = currentWeapon.GetWeaponData().damage,
            DamageType = DamageType.Heavy,
            HitDirection = (other.transform.position - transform.position).normalized,
            HitPosition = other.ClosestPoint(hitbox.transform.position),
        });
    }
}
