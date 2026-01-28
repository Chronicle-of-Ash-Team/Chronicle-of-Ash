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

    [Header("Roll Settings")]
    public bool isRolling = false;
    public bool isAttacking = false;
    public float rollSpeed = 8f;
    public int attackComboCount = 1;
    Vector3 rollDirection;

    public event Action OnDodge;
    public event Action<int> OnAttack;

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

    private void TryAttack()
    {
        if (isRolling) return;
        isAttacking = true;
        if(attackComboCount == 3)
        {
            attackComboCount = 1;
        }
        else
        {
            attackComboCount++;
        }
        OnAttack?.Invoke(attackComboCount);
    }

    private void TryDodge()
    {
        if (isRolling) return;

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
    }
    public void EndRoll()
    {
        isRolling = false;
        rb.linearVelocity = Vector3.zero;
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
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
