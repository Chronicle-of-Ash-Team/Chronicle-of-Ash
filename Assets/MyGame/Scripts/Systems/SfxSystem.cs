using UnityEngine;

public class SfxSystem : MonoBehaviour
{
    [SerializeField] private Hit_Event hitEvent;
    [SerializeField] private Parry_Event parryEvent;
    [SerializeField] private FoorStep_Event footStepEvent;

    [SerializeField] private GameObject hitVfxPrefab;
    [SerializeField] private GameObject parryVfxPrefab;
    [SerializeField] private GameObject footStepVfxPrefab;

    private void OnEnable()
    {
        hitEvent.OnEventRaised += SpawnHitSfx;
        parryEvent.OnEventRaised += SpawnParrySfx;
        footStepEvent.OnEventRaised += SpawnFootStepSfx;
    }

    private void SpawnFootStepSfx(FootStepContext context)
    {
        GameObject sfx = ObjectPoolManager.Instance.Spawn(footStepVfxPrefab, context.stepPlayer.position, Quaternion.identity);
        sfx.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.2f);
    }

    private void OnDisable()
    {
        hitEvent.OnEventRaised -= SpawnHitSfx;
        parryEvent.OnEventRaised -= SpawnParrySfx;
        footStepEvent.OnEventRaised -= SpawnFootStepSfx;
    }

    private void SpawnParrySfx(DamageContext context)
    {
        GameObject sfx = ObjectPoolManager.Instance.Spawn(parryVfxPrefab, context.HitPosition, Quaternion.LookRotation(context.HitDirection));
        foreach (var audio in sfx.GetComponentsInChildren<AudioSource>())
        {
            audio.pitch = Random.Range(0.8f, 1.5f);
        }
        sfx.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.5f);
    }

    private void SpawnHitSfx(DamageContext context)
    {
        GameObject sfx = ObjectPoolManager.Instance.Spawn(hitVfxPrefab, context.HitPosition, Quaternion.LookRotation(context.HitDirection));
        sfx.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.2f);
    }
}
