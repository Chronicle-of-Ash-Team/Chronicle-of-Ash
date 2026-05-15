using UnityEngine;

public class VfxSystem : MonoBehaviour
{
    [SerializeField] private Hit_Event hitEvent;
    [SerializeField] private Parry_Event parryEvent;


    [SerializeField] private GameObject hitVfxPrefab;
    [SerializeField] private GameObject parryVfxPrefab;

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
        ObjectPoolManager.Instance.Spawn(hitVfxPrefab, context.HitPosition, Quaternion.LookRotation(context.HitDirection));
    }

}
