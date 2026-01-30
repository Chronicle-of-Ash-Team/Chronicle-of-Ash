using System;
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


    public event Action OnDodge;
    public event Action<int> OnAttack;
    public event Action OnSkill;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameInput.Instance.OnDodgePerformed += GameInput_OnDodgePerformed;
        GameInput.Instance.OnAttackPerformed += GameInput_OnAttackPerformed;
    }

    private void GameInput_OnAttackPerformed()
    {
        TryAttack();
    }

    private void GameInput_OnDodgePerformed()
    {
        TryDodge();
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
        OnSkill?.Invoke();
    }

    private void TryAttack()
    {
        if (isRolling || isAttacking) return;
        isAttacking = true;
        comboTimer = 0f;

        attackComboCount++;

        if (attackComboCount > 3)
        {
            attackComboCount = 1;
        }
        OnAttack?.Invoke(attackComboCount);
    }

    private void TryDodge()
    {
        if (isRolling || isDamaging) return;

        isAttacking = false;

        OnDodge?.Invoke();

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

    public void StartRoll()
    {
        isRolling = true;
        isAttacking = false;
    }
    public void EndRoll()
    {
        isRolling = false;
        isAttacking = false;
        rb.linearVelocity = Vector3.zero;
    }

    public void StartAttack()
    {
        isAttacking = true;
        isDamaging = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
        isDamaging = false;
    }


    public bool GetIsRolling()
    {
        return isRolling;
    }
    public bool GetIsAttacking()
    {
        return isAttacking;
    }

}
