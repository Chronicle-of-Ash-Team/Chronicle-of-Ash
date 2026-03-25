using UnityEngine;
using UnityEngine.UI;

public class RuntimeUI : MonoBehaviour
{
    [SerializeField] private Image lockOnImg;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider staminaSlider;

    [SerializeField] private HPChanged_Event HPChanged_Event;

    private Transform lockOnTarget;

    private void Awake()
    {
        lockOnImg.enabled = false;
    }

    private void Start()
    {
        UIEvents.OnTargetLock += OnTargetLock;

        HPChanged_Event.OnEventRaised += OnHPChanged;
    }

    private void OnHPChanged(HPContext context)
    {
        hpSlider.value = context.CurrentHP / (float)context.MaxHP;
    }

    private void OnTargetLock(Transform obj)
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

            //Ray ray = new Ray(Camera.main.transform.position,
            //      lockOnTarget.position - Camera.main.transform.position);

            //if (Physics.Raycast(ray, out RaycastHit hit))
            //{
            //    if (hit.transform.GetComponentInParent<Transform>() != lockOnTarget.GetComponentInParent<Transform>())
            //        lockOnImg.enabled = false;
            //}
        }
    }
}
