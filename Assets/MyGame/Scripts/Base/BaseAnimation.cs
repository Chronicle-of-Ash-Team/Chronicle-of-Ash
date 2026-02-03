using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BaseAnimation : MonoBehaviour
{
    protected Animator animator;
    Coroutine upperBodyBlendRoutine;

    protected readonly int moveAmountHash = Animator.StringToHash("MoveAmount");
    protected readonly int isMovingHash = Animator.StringToHash("IsMoving");
    protected readonly int isLockOnHash = Animator.StringToHash("IsLockOn");
    protected readonly int horizontalHash = Animator.StringToHash("Horizontal");
    protected readonly int verticalHash = Animator.StringToHash("Vertical");
    protected readonly int speedMultiplyHash = Animator.StringToHash("SpeedMultiply");

    public event Action OnAttackStart;
    public event Action OnAttackEnd;
    public event Action OnDodgeStart;
    public event Action OnDodgeEnd;
    public event Action OnHitStart;
    public event Action OnHitEnd;

    public event Action OnActionEventStart;
    public event Action OnActionEventEnd;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected void ApplyAnimatorOverride()
    {

    }

    public void ApplyWeapon(WeaponBase obj)
    {
        if (obj.animatorOverride != null)
        {
            animator.runtimeAnimatorController = obj.animatorOverride;
        }
        animator.SetFloat(speedMultiplyHash, obj.speed);
    }

    public void PlayThisAnimation(string animationName, float blendTime = 0.08f)
    {
        StopBlendUpper();
        animator.CrossFade(animationName, blendTime);
    }

    public void PlayThisAnimationWithUpper(string animationName, float blendTime = 0f)
    {
        animator.Play(animationName, 0, blendTime);
    }

    public virtual void UpdateLocomotionAnimation(float normalizedSpeed)
    {
        animator.SetBool(isLockOnHash, false);
        animator.SetFloat(moveAmountHash, normalizedSpeed, 0.2f, Time.deltaTime);
        animator.SetBool(isMovingHash, normalizedSpeed > 0.1f);
    }
    public virtual void UpdateLockOnLocomotion(Vector3 worldMoveDir)
    {
        animator.SetBool(isLockOnHash, true);

        Vector3 localMove = transform.InverseTransformDirection(worldMoveDir);

        animator.SetFloat(horizontalHash, localMove.x, 0.15f, Time.deltaTime);
        animator.SetFloat(verticalHash, localMove.z, 0.15f, Time.deltaTime);

        animator.SetBool(isMovingHash, localMove.magnitude > 0.1f);
    }

    protected void BlendUpper()
    {
        if (upperBodyBlendRoutine != null)
            StopCoroutine(upperBodyBlendRoutine);

        upperBodyBlendRoutine = StartCoroutine(BlendUpperBody(1, 0.25f));
    }

    protected void StopBlendUpper()
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

    protected void OnAttackStartEvent()
    {
        OnAttackStart?.Invoke();
    }
    protected void OnAttackEndEvent()
    {
        OnAttackEnd?.Invoke();
    }
    protected void OnDodgeStartEvent()
    {
        OnDodgeStart?.Invoke();
    }
    protected void OnDodgeEndEvent()
    {
        OnDodgeEnd?.Invoke();
    }
    protected void OnHitStartEvent()
    {
        OnHitStart?.Invoke();
    }
    protected void OnHitEndEvent()
    {
        OnHitEnd?.Invoke();
    }
    protected void OnActionEventStartEvent()
    {
        OnActionEventStart?.Invoke();
    }
    protected void OnActionEventEndEvent()
    {
        OnActionEventEnd?.Invoke();
    }
}
