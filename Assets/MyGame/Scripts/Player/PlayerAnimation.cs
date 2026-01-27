using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private readonly int moveAmountHash = Animator.StringToHash("MoveAmount");
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int isLockOnHash = Animator.StringToHash("IsLockOn");
    private readonly int horizontalHash = Animator.StringToHash("Horizontal");
    private readonly int verticalHash = Animator.StringToHash("Vertical");

    private void Start()
    {
        PlayerCombat.Instance.OnDodge += PlayerCombat_OnDodge;
    }

    private void PlayerCombat_OnDodge()
    {
        //animator.SetTrigger(rollHash);
        animator.CrossFade("Roll", 0.02f);
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
    public Animator GetAnimator()
    {
        return animator;
    }
}
