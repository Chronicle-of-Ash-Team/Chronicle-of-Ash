using UnityEngine;
using UnityEngine.UI;

public class RuntimeUI : MonoBehaviour
{
    [SerializeField] private Image lockOnImg;

    private Transform lockOnTarget;

    private void Awake()
    {
        lockOnImg.enabled = false;
    }

    private void Start()
    {
        PlayerTargetLock.Instance.OnTargetLock += PlayerTargetLock_OnTargetLock;
    }

    private void PlayerTargetLock_OnTargetLock(Transform obj)
    {
        lockOnTarget = obj;
    }

    private void LateUpdate()
    {
        if (lockOnTarget == null)
        {
            lockOnImg.enabled = false;
        }
        else
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(lockOnTarget.position);
            lockOnImg.enabled = true;
            lockOnImg.transform.position = screenPos;

            Ray ray = new Ray(Camera.main.transform.position,
                  lockOnTarget.position - Camera.main.transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != lockOnTarget)
                    lockOnImg.enabled = false;
            }
        }
    }
}
