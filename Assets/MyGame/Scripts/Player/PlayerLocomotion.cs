using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraTransform;
    private Vector3 cameraOriginPosition;

    [SerializeField] private float moveSpeed = 7f;

    void Start()
    {
        cameraOriginPosition = cameraTransform.localPosition;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 inputDir = new Vector3(inputVector.x, 0f, inputVector.y);
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 moveDir = (cameraForward * inputDir.z + cameraRight * inputDir.x).normalized * moveSpeed;
        moveDir.y = 0f;

        // Di chuyển
        //transform.position += moveDir;
        //rb.AddForce(moveDir);
        rb.linearVelocity = moveDir;
        //cameraTransform.localPosition = cameraOriginPosition;

        //transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}
