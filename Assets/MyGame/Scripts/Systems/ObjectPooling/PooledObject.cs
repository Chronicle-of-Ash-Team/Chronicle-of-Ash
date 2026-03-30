using System.Collections;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private GameObject prefab;
    private ObjectPoolManager pool;

    [Header("Auto Return")]
    [SerializeField] private bool autoReturn = true;
    [SerializeField] private float fallbackDelay = 2f;

    private ParticleSystem[] particleSystems;
    private Coroutine returnCoroutine;

    public void Init(GameObject prefab, ObjectPoolManager pool)
    {
        this.prefab = prefab;
        this.pool = pool;

        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    private void OnEnable()
    {
        if (autoReturn)
        {
            StartReturnRoutine();
        }
    }

    private void OnDisable()
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
    }

    void StartReturnRoutine()
    {
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        returnCoroutine = StartCoroutine(ReturnWhenDone());
    }

    IEnumerator ReturnWhenDone()
    {
        if (particleSystems != null && particleSystems.Length > 0)
        {
            yield return new WaitUntil(() =>
            {
                foreach (var ps in particleSystems)
                {
                    if (ps != null && ps.IsAlive(true))
                        return false;
                }
                return true;
            });
        }
        else
        {
            yield return new WaitForSeconds(fallbackDelay);
        }

        ReturnToPool();
    }

    public void ReturnToPool()
    {
        pool.Despawn(gameObject, prefab);
    }
}