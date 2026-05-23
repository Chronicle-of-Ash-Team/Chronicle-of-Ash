using UnityEngine;

public class VfxSystem : MonoBehaviour
{
    [SerializeField] private Hit_Event hitEvent;
    [SerializeField] private Parry_Event parryEvent;

    [SerializeField] private GameObject hitVfxPrefab;
    public GameObject BloodAttach;
    public GameObject[] BloodFX;

    [SerializeField] private GameObject parryVfxPrefab;

    int effectIdx;

    private void OnEnable()
    {
        hitEvent.OnEventRaised += SpawnHitVfx;
        parryEvent.OnEventRaised += SpawnParryVfx;
    }
    private void OnDisable()
    {
        hitEvent.OnEventRaised -= SpawnHitVfx;
        parryEvent.OnEventRaised -= SpawnParryVfx;
    }

    private void SpawnParryVfx(DamageContext context)
    {
        ObjectPoolManager.Instance.Spawn(parryVfxPrefab, context.HitPosition, Quaternion.LookRotation(context.HitDirection));
    }

    private void SpawnHitVfx(DamageContext context)
    {
        Vector3 dir = context.HitDirection.normalized;

        Vector3 rayOrigin = context.HitPosition + dir * 2f;

        if (Physics.Raycast(rayOrigin, -dir, out RaycastHit hit, 5f))
        {
            if (effectIdx >= BloodFX.Length)
                effectIdx = 0;

            float angle = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180;

            ObjectPoolManager.Instance.Spawn(
                BloodFX[effectIdx],
                hit.point,
                Quaternion.Euler(0, angle + 90, 0)
            );

            effectIdx++;
        }
    }
}
