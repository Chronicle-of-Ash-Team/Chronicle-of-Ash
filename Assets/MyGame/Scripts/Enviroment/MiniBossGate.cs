using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MiniBossGate : MonoBehaviour
{
    [SerializeField] private BaseBoss boss;
    [SerializeField] private GameObject blockGate;

    private void Start()
    {
        if (boss == null)
        {
            Debug.LogError("MiniBoss reference is not set in the inspector.");
            return;
        }
        blockGate.SetActive(false);

        boss.OnBossDie += HandleBossDeath;
    }

    private void OnDestroy()
    {
        if (boss != null)
        {
            boss.OnBossDie -= HandleBossDeath;
        }
    }

    private void HandleBossStart()
    {
        blockGate.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<IDamageable>() != null)
        {
            HandleBossStart();
        }
    }

    private void HandleBossDeath()
    {
        Destroy(gameObject);
    }
}
