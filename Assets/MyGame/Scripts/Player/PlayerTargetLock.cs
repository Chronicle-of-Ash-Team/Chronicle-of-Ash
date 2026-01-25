using Unity.Cinemachine;
using UnityEngine;

public class PlayerTargetLock : MonoBehaviour
{
    public static PlayerTargetLock Instance;

    [SerializeField] private float radius = 20f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private GameObject cinemachineFreeLook;
    [SerializeField] private GameObject cinemachineLockon;

    public bool isTargeting;

    private Transform currentTarget;
    private float mouseX;
    private float mouseY;

    private CinemachineCamera freelookCinemachineCamera;
    private CinemachineCamera lockonCinemachineCamera;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameInput.Instance.OnLockOnPerformed += GameInput_OnLockOnPerformed;

        freelookCinemachineCamera = cinemachineFreeLook.GetComponent<CinemachineCamera>();
        lockonCinemachineCamera = cinemachineLockon.GetComponent<CinemachineCamera>();
    }

    private void OnEnable()
    {
    }

    private void GameInput_OnLockOnPerformed()
    {
        TryLockOn();
    }

    void Update()
    {
        //ClosestTarget();
        HandleLockOn();
        if (isTargeting)
        {
            cinemachineFreeLook.transform.position = cinemachineLockon.transform.position;
            cinemachineFreeLook.transform.rotation = cinemachineLockon.transform.rotation;
        }
    }

    private void TryLockOn()
    {
        if (isTargeting)
        {
            // Unlock
            isTargeting = false;
            currentTarget = null;

            freelookCinemachineCamera.ForceCameraPosition(lockonCinemachineCamera.State.GetFinalPosition(), lockonCinemachineCamera.State.GetFinalOrientation());
            cinemachineFreeLook.SetActive(true);
        }
        else
        {
            // Lock
            GameObject target = ClosestTarget();
            if (target != null)
            {
                isTargeting = true;
                currentTarget = target.transform;

                lockonCinemachineCamera.ForceCameraPosition(freelookCinemachineCamera.State.GetFinalPosition(), freelookCinemachineCamera.State.GetFinalOrientation());

                cinemachineFreeLook.SetActive(false);
            }
        }
    }

    private void HandleLockOn()
    {
        if (currentTarget == null || cameraTarget == null) return;

        // Tính hướng nhìn vào target
        Vector3 directionToTarget = currentTarget.position - cameraTarget.position;
        directionToTarget.y = 0; // Giữ camera level

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

    private GameObject ClosestTarget()
    {
        GameObject closestTarget = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<ILockable>(out var lockable))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = collider.gameObject;
                }
            }
        }

        if (closestTarget != null)
        {
            Debug.DrawLine(transform.position, closestTarget.transform.position, Color.green, .1f);
            Debug.Log($"Closest target: {closestTarget.name} at {closestDistance:F2}m");
        }

        return closestTarget;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);
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
