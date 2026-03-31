using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 50f;
    [SerializeField] private float waitBeforeMove = 0.5f;
    [SerializeField] private GameObject hitEffectPrefab;

    private GameObject owner;
    private Vector3 direction;
    private int damage;

    private float lifeTimer;

    private void OnEnable()
    {
        lifeTimer = lifeTime;
    }

    public void Init(GameObject owner, Vector3 direction, int damage)
    {
        this.owner = owner;
        this.direction = direction;
        this.damage = damage;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= lifeTime - waitBeforeMove)
        {
            Move();
        }
        else if (lifeTimer <= 0f)
        {
            Despawn();
            return;
        }
    }

    private void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            if (damageable == owner.GetComponent<IDamageable>())
            {
                return;
            }
            damageable.TakeDamage(new DamageContext
            {
                Attacker = owner,
                Damage = damage,
                HitDirection = direction,
                HitPosition = transform.position,
                DamageType = DamageType.Normal
            });
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }
        }

        Despawn();
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }
}
