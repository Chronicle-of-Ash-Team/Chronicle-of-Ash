using UnityEngine;

public class RedKnight : BaseLocomotion, ILockable, IActionHandler, IDamageable, IWeaponOwner
{
    [Header("Common Settings")]
    [SerializeField] private Transform lockOnPos;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float speedMultiplier = 0.4f;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Attack Settings")]
    [SerializeField] private GameObject weaponPref;
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange = 2.2f;
    //[SerializeField] private float stopDistance = 1.8f;

    [Header("Action Settings")]
    public BaseCombatAction CurrentAction { get; private set; }
    private AttackAction attackAction;
    private WeaponController currentWeapon;
    private HitAction hitAction;

    private PlayerAnimation playerAnimation;

    protected override void Awake()
    {
        base.Awake();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        hitAction = GetComponent<HitAction>();
        attackAction = GetComponent<AttackAction>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        if (weaponPref != null)
        {
            GameObject weapon = Instantiate(weaponPref, weaponHolder);
            currentWeapon = weapon.GetComponent<WeaponController>();
            currentWeapon.Init(this);
        }

        currentWeapon.GetWeaponData().speed = speedMultiplier;

        playerAnimation.ApplyWeapon(currentWeapon.GetWeaponData());

        playerAnimation.OnAttackStart += PlayerAnimation_OnAttackStart;
        playerAnimation.OnAttackEnd += PlayerAnimation_OnAttackEnd;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            target = null;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        FaceTarget(target);

        float currentSpeed = 0f;

        if (distance > attackRange)
        {
            currentSpeed = walkSpeed;
            Vector3 moveDir = GetMoveDirToTarget(transform, target);
            HandleMovement(moveDir, false);
        }
        else
        {
            // Trong tầm đánh
            HandleMovement(Vector3.zero, false);
            TryAction(attackAction);
        }
        playerAnimation.UpdateLocomotionAnimation(currentSpeed);
    }


    public void TryAction(BaseCombatAction action)
    {
        if (action == hitAction)
        {
            if (CurrentAction != null)
            {
                CurrentAction.OnFinish();
                CurrentAction = null;
            }

            action.TryExecute();
            CurrentAction = action;

            Debug.Log("Enemy hit");
            return;
        }


        if (CurrentAction != null && CurrentAction.IsRunning)
            return;
        // Không chạy lại cùng một action
        if (CurrentAction == action)
            return;

        // Execute action mới
        CurrentAction?.OnFinish();
        action.TryExecute();
        CurrentAction = action;
    }

    public void OnActionFinished(BaseCombatAction action)
    {
        if (CurrentAction == action)
            CurrentAction = null;
    }

    public void TakeDamage(DamageContext damageContext)
    {
        //TryAction(hitAction);
        currentHealth -= damageContext.Damage;
        var checkAttacker = damageContext.Attacker.GetComponent<IDamageable>();
        if (checkAttacker != null)
        {
            target = damageContext.Attacker.transform;
        }
    }


    private Vector3 GetMoveDirToTarget(Transform self, Transform target)
    {
        Vector3 dir = target.position - self.position;
        dir.y = 0f;
        return dir.normalized;
    }

    private void FaceTarget(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * 10f
        );
    }
    public int GetDamage()
    {
        return currentWeapon.GetWeaponData().damage;
    }

    public WeaponBase GetWeaponData()
    {
        return currentWeapon.GetWeaponData();
    }
    public Transform GetLockOnTransform()
    {
        return lockOnPos;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SetInvincible(bool invincible)
    {
        //throw new System.NotImplementedException();
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
