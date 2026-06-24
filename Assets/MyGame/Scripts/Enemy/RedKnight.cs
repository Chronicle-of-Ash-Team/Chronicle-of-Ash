using UnityEngine;

public class RedKnight : BaseBoss, IActionHandler, IWeaponOwner
{
    [Header("Common Settings")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float speedMultiplier = 0.4f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject weaponPref;
    [SerializeField] private float attackRange = 2.2f;

    public BaseCombatAction CurrentAction { get; private set; }
    private AttackAction attackAction;
    private WeaponController currentWeapon;
    private HitAction hitAction;

    private BaseAnimation playerAnimation;

    protected override void Awake()
    {
        base.Awake();
        playerAnimation = GetComponentInChildren<BaseAnimation>();
        hitAction = GetComponent<HitAction>();
        attackAction = GetComponent<AttackAction>();
    }

    private void Start()
    {
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
        if (!isAlive) return;
        if (target == null) return;

        if (!target.GetComponent<IDamageable>().GetIsAlive())
        {
            target = null;
            return;
        }

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
        if (!isAlive) return;
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
        if (target.GetIsAlive() == false) return;
        target.TakeDamage(new DamageContext
        {
            Attacker = gameObject,
            Damage = 2,
            DamageType = DamageType.Heavy,
            HitDirection = (other.transform.position - transform.position).normalized,
            HitPosition = other.ClosestPoint(hitbox.transform.position),
        });
    }
}
