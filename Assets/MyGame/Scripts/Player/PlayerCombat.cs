using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;
    public enum PlayerState
    {
        Idle,
        Combat,
    }
    public PlayerState CurrentState;

    private Rigidbody rb;

    [Header("Attack Settings")]
    [SerializeField] private float comboResetTime = 1.2f;
    [SerializeField] bool isAttacking = false;
    [SerializeField] int attackComboCount = 1;
    [SerializeField] bool isDamaging = false;

    private float comboTimer;

    [Header("Roll Settings")]
    [SerializeField] bool isRolling = false;
    [SerializeField] float rollSpeed = 8f;
    Vector3 rollDirection;


    private IMove playerLocomotion;
    private PlayerAnimation playerAnimation;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerLocomotion = GetComponent<IMove>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();

        GameInput.Instance.OnDodgePerformed += GameInput_OnDodgePerformed;
        GameInput.Instance.OnAttackPerformed += GameInput_OnAttackPerformed;

        playerAnimation.OnAttackStart += PlayerAnimation_OnAttackStart;
        playerAnimation.OnAttackEnd += PlayerAnimation_OnAttackEnd;
        playerAnimation.OnDodgeStart += PlayerAnimation_OnDodgeStart;
        playerAnimation.OnDodgeEnd += PlayerAnimation_OnDodgeEnd;
    }

    private void Update()
    {
        HandleComboTimer();
        if (Input.GetKeyDown(KeyCode.T))
        {
            TrySkill();
        }
    }

    private void FixedUpdate()
    {
        HandleDodge();
    }

    private void HandleDodge()
    {
        if (!isRolling) return;
        rb.linearVelocity = rollDirection * rollSpeed;
        transform.forward = rollDirection;
    }

    private void HandleComboTimer()
    {
        if (attackComboCount == 0) return;

        comboTimer += Time.deltaTime;

        if (comboTimer >= comboResetTime)
        {
            attackComboCount = 0;
            comboTimer = 0f;
        }
    }

    private void TrySkill()
    {
        isAttacking = true;
        playerLocomotion.StopMove();
        playerAnimation.PlaySkill();
    }

    private void TryAttack()
    {
        if (isRolling || isAttacking) return;
        isAttacking = true;
        playerLocomotion.StopMove();
        comboTimer = 0f;

        attackComboCount++;

        if (attackComboCount > 3)
        {
            attackComboCount = 1;
        }
        playerAnimation.PlayAttack(attackComboCount);
    }

    private void TryDodge()
    {
        if (isRolling || isDamaging) return;

        isAttacking = false;
        playerLocomotion.StopMove();

        playerAnimation.PlayDodge();

        Vector2 moveInput = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 moveDir = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        if (moveDir.sqrMagnitude < 0.01f)
        {
            moveDir = transform.forward;
        }

        rollDirection = moveDir.normalized;
        transform.forward = rollDirection;
    }

    private void PlayerAnimation_OnDodgeStart()
    {
        isRolling = true;
        isAttacking = false;
        playerLocomotion.StopMove();
    }

    private void PlayerAnimation_OnDodgeEnd()
    {
        isRolling = false;
        isAttacking = false;
        rb.linearVelocity = Vector3.zero;
        playerLocomotion.ResumeMove();
    }

    private void PlayerAnimation_OnAttackEnd()
    {
        isAttacking = false;
        isDamaging = false;
        playerLocomotion.ResumeMove();
    }

    private void PlayerAnimation_OnAttackStart()
    {
        isAttacking = true;
        isDamaging = true;
        playerLocomotion.StopMove();
    }

    private void GameInput_OnAttackPerformed()
    {
        TryAttack();
    }

    private void GameInput_OnDodgePerformed()
    {
        TryDodge();
    }
}
