using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    Coroutine upperBodyBlendRoutine;

    private readonly int moveAmountHash = Animator.StringToHash("MoveAmount");
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int isLockOnHash = Animator.StringToHash("IsLockOn");
    private readonly int horizontalHash = Animator.StringToHash("Horizontal");
    private readonly int verticalHash = Animator.StringToHash("Vertical");
    private readonly int speedMultiplyHash = Animator.StringToHash("SpeedMultiply");

    public event Action OnAttackStart;
    public event Action OnAttackEnd;
    public event Action OnDodgeStart;
    public event Action OnDodgeEnd;
    public event Action OnHitStart;
    public event Action OnHitEnd;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ApplyWeapon(WeaponBase obj)
    {
        if (obj.animatorOverride != null)
        {
            animator.runtimeAnimatorController = obj.animatorOverride;
        }
        animator.SetFloat(speedMultiplyHash, obj.speed);
    }

    public void PlayAttack(int comboIndex)
    {
        StopBlendUpper();
        animator.CrossFade("Attack" + comboIndex.ToString(), 0.08f);
    }

    public void PlaySkill()
    {
        StopBlendUpper();
        animator.CrossFade("Skill", 0.08f);
    }

    public void PlayDodge()
    {
        StopBlendUpper();
        animator.CrossFade("Roll", 0.02f);
    }

    public void PlayHit()
    {
        StopBlendUpper();
        animator.CrossFade("Hit", 0f);
    }

    public void UpdateLocomotionAnimation(float normalizedSpeed)
    {
        animator.SetBool(isLockOnHash, false);
        animator.SetFloat(moveAmountHash, normalizedSpeed, 0.2f, Time.deltaTime);
        animator.SetBool(isMovingHash, normalizedSpeed > 0.1f);
    }
    public void UpdateLockOnLocomotion(Vector3 worldMoveDir)
    {
        animator.SetBool(isLockOnHash, true);

        Vector3 localMove = transform.InverseTransformDirection(worldMoveDir);

        animator.SetFloat(horizontalHash, localMove.x, 0.15f, Time.deltaTime);
        animator.SetFloat(verticalHash, localMove.z, 0.15f, Time.deltaTime);

        animator.SetBool(isMovingHash, localMove.magnitude > 0.1f);
    }

    private void StopBlendUpper()
    {
        if (upperBodyBlendRoutine != null)
            StopCoroutine(upperBodyBlendRoutine);

        animator.SetLayerWeight(1, 0f);
    }

    IEnumerator BlendUpperBody(float target, float duration)
    {
        int layer = 1;
        float start = animator.GetLayerWeight(layer);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            animator.SetLayerWeight(layer, Mathf.Lerp(start, target, t));
            yield return null;
        }

        animator.SetLayerWeight(layer, target);
    }

    private void StartRoll()
    {
        OnDodgeStart?.Invoke();
    }
    private void EndRoll()
    {
        OnDodgeEnd?.Invoke();
    }
    private void StartAttack()
    {
        animator.SetLayerWeight(1, 0f);
        OnAttackStart?.Invoke();
    }
    private void EndAttack()
    {
        if (upperBodyBlendRoutine != null)
            StopCoroutine(upperBodyBlendRoutine);

        upperBodyBlendRoutine = StartCoroutine(BlendUpperBody(1, 0.25f));
        OnAttackEnd?.Invoke();
    }
    private void StartHit()
    {
        OnHitStart?.Invoke();
    }
    private void EndHit()
    {
        OnHitEnd?.Invoke();
    }
}
