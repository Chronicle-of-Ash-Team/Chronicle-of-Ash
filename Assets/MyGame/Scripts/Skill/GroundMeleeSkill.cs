using UnityEngine;

public class GroundMeleeSkill : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveFromY;
    [SerializeField] private float moveToY;
    [SerializeField] private float moveTime = 0.1f;
    [SerializeField] private float stayTime = 0.3f; // thời gian đứng

    [Header("Damage")]
    [SerializeField] private int damage = 10;

    private float timer;
    private bool isMoving;
    private bool isStaying;

    private void OnEnable()
    {
        transform.position = new Vector3(
            transform.position.x,
            moveFromY,
            transform.position.z
        );

        timer = 0f;
        isMoving = true;
        isStaying = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Phase 1: Move lên
        if (isMoving)
        {
            float t = timer / moveTime;

            transform.position = new Vector3(
                transform.position.x,
                Mathf.Lerp(moveFromY, moveToY, t),
                transform.position.z
            );

            if (t >= 1f)
            {
                isMoving = false;
                isStaying = true;
                timer = 0f; // reset timer cho phase đứng
            }
        }
        // Phase 2: Đứng yên
        else if (isStaying)
        {
            if (timer >= stayTime)
            {
                isStaying = false;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Gọi hệ thống damage của bạn
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(new DamageContext
            {
                Damage = damage,
                Attacker = gameObject,
                HitDirection = (other.transform.position - transform.position).normalized,
                HitPosition = other.ClosestPoint(transform.position),
                DamageType = DamageType.Normal
            });
        }

        // Nếu chỉ muốn hit 1 lần rồi biến mất
        //gameObject.SetActive(false);

    }
}