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

    private void Start()
    {
        animator = GetComponent<Animator>();
        PlayerCombat.Instance.OnDodge += PlayerCombat_OnDodge;
        PlayerCombat.Instance.OnAttack += PlayerCombat_OnAttack;

        PlayerWeapon.Instance.OnWeaponChanged += PlayerWeapon_OnWeaponChanged;
    }

    private void PlayerWeapon_OnWeaponChanged(WeaponData obj)
    {
        Debug.Log("Current Sword: " + obj.name);
        if (obj.animatorOverride != null)
        {
            animator.runtimeAnimatorController = obj.animatorOverride;
        }
        animator.SetFloat(speedMultiplyHash, obj.speed);
    }

    private void PlayerCombat_OnAttack(int obj)
    {
        if (upperBodyBlendRoutine != null)
            StopCoroutine(upperBodyBlendRoutine);

        animator.SetLayerWeight(1, 0f);

        animator.CrossFade("Attack" + obj.ToString(), 0.08f);
    }

    private void PlayerCombat_OnDodge()
    {
        if (upperBodyBlendRoutine != null)
            StopCoroutine(upperBodyBlendRoutine);

        animator.SetLayerWeight(1, 0f);

        animator.CrossFade("Roll", 0.02f);
    }

    private void LateUpdate()
    {
        if (PlayerTargetLock.Instance.GetIsTargeting())
        {
            UpdateLockOnLocomotion(PlayerLocomotion.Instance.GetCurrentMoveDir());
        }
        else
        {
            UpdateLocomotionAnimation(PlayerLocomotion.Instance.GetNormalizedSpeed());
        }
    }

    public void UpdateLocomotionAnimation(float normalizedSpeed)
    {
        // Smooth transition với damping time 0.2s
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

    public void SetMoveAmountImmediate(float value)
    {
        animator.SetFloat(moveAmountHash, value);
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


    public Animator GetAnimator()
    {
        return animator;
    }

    public void StartRoll()
    {
        PlayerCombat.Instance.StartRoll();
    }
    public void EndRoll()
    {
        PlayerCombat.Instance.EndRoll();
    }
    public void StartAttack()
    {
        animator.SetLayerWeight(1, 0f);
        PlayerCombat.Instance.StartAttack();
    }
    public void EndAttack()
    {
        if (upperBodyBlendRoutine != null)
            StopCoroutine(upperBodyBlendRoutine);

        upperBodyBlendRoutine = StartCoroutine(BlendUpperBody(1, 0.25f));
        PlayerCombat.Instance.EndAttack();
    }
}
