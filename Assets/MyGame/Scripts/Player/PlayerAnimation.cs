using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private readonly int moveAmountHash = Animator.StringToHash("MoveAmount");
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateLocomotionAnimation(float normalizedSpeed)
    {
        // Smooth transition với damping time 0.2s
        animator.SetFloat(moveAmountHash, normalizedSpeed, 0.2f, Time.deltaTime);
        animator.SetBool(isMovingHash, normalizedSpeed > 0.1f);
    }
    public void SetMoveAmountImmediate(float value)
    {
        animator.SetFloat(moveAmountHash, value);
    }
}
