using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerTargetLock : MonoBehaviour
{
    public static PlayerTargetLock Instance;

    [SerializeField] private float lockDistance = 10f;
    [SerializeField] private float cancelDistance = 20f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineCamera freelookCinemachineCamera;
    [SerializeField] private CinemachineCamera lockonCinemachineCamera;

    private bool isTargeting;
    private Transform currentTarget;


    public event Action<Transform> OnTargetLock;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameInput.Instance.OnLockOnPerformed += GameInput_OnLockOnPerformed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void GameInput_OnLockOnPerformed()
    {
        TryLockOn();
    }

    void Update()
    {
        HandleLockOn();
    }

    private void TryLockOn()
    {
        CinemachineThirdPersonFollow cinemachineThirdPerson = lockonCinemachineCamera.GetComponent<CinemachineThirdPersonFollow>();
        if (isTargeting)
        {
            // Unlock
            cinemachineThirdPerson.enabled = false;
            isTargeting = false;
            currentTarget = null;

            freelookCinemachineCamera.ForceCameraPosition(lockonCinemachineCamera.State.GetFinalPosition(), lockonCinemachineCamera.State.GetFinalOrientation());
        }
        else
        {
            // Lock
            GameObject target = BestTarget();
            if (target != null)
            {
                cinemachineThirdPerson.enabled = true;
                isTargeting = true;
                currentTarget = target.transform;

                lockonCinemachineCamera.ForceCameraPosition(freelookCinemachineCamera.State.GetFinalPosition(), freelookCinemachineCamera.State.GetFinalOrientation());
            }
        }
        OnTargetLock?.Invoke(currentTarget);
    }

    private void HandleLockOn()
    {
        if (currentTarget == null || cameraTarget == null) return;

        if (Vector3.Distance(transform.position, currentTarget.position) > cancelDistance)
        {
            TryLockOn();
            return;
        }

        // Tính hướng nhìn vào target
        Vector3 directionToTarget = currentTarget.position - cameraTarget.position;
        directionToTarget.y = 0;

        if (directionToTarget.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Smooth rotate camera target
            cameraTarget.rotation = Quaternion.Slerp(
                cameraTarget.rotation,
                targetRotation,
                2 * Time.deltaTime
            );
        }
    }

    private GameObject BestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, lockDistance);

        GameObject bestTarget = null;
        float bestScore = float.MaxValue;

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<ILockable>(out var lockable))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // Lấy lock point
                Transform lockPoint = lockable.lockPos();
                Vector3 targetPosition = lockPoint != null ? lockPoint.position : collider.transform.position;

                // Tính hướng tới target
                Vector3 directionToTarget = (targetPosition - transform.position);
                directionToTarget.y = 0;
                directionToTarget.Normalize();

                // Tính góc
                float angle = Vector3.Angle(cameraForward, directionToTarget);

                // Chỉ xét targets trong góc nhìn
                if (angle > 60f) continue;

                // Score: ưu tiên targets ở giữa màn hình + gần
                float score = (angle * 2f) + (distance * 1.5f);

                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = collider.gameObject;
                }
            }
        }
        return bestTarget;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, lockDistance);
    }

    public bool GetIsTargeting()
    {
        return isTargeting;
    }
    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }
}
