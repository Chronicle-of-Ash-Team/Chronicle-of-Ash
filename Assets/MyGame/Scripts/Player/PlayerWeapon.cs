using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform rightHandHolder;

    [Header("Weapon Setting")]
    public WeaponBase currentWeapon;

    [SerializeField] private float maxDistance = 5f;

    private PlayerAnimation playerAnimation;

    private void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update()
    {
        HandleInteract();
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeWeapon(currentWeapon);
        }
    }

    private void ChangeWeapon(WeaponBase weaponData)
    {
        currentWeapon = weaponData;
        foreach (Transform chil in rightHandHolder.transform)
        {
            Destroy(chil.gameObject);
        }
        Instantiate(weaponData.weaponPrefab, rightHandHolder);
        playerAnimation.ApplyWeapon(currentWeapon);
    }

    private void HandleInteract()
    {
        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
        {
            var weapon = hit.transform.GetComponentInParent<IWeaponProp>();
            if (weapon != null && Input.GetKeyDown(KeyCode.E))
            {
                ChangeWeapon(weapon.GetWeaponData());
            }
        }
    }
}
