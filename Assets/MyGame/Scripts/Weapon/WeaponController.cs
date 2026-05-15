using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private WeaponBase weaponData;
    private WeaponHitBox weaponHitBox;
    private IWeaponOwner owner;

    private void Awake()
    {
        weaponHitBox = GetComponentInChildren<WeaponHitBox>();
    }

    public void Init(IWeaponOwner owner)
    {
        this.owner = owner;
        weaponHitBox.Init(owner);
    }

    public WeaponBase GetWeaponData() { return weaponData; }
    public WeaponHitBox GetWeaponHitBox() { return weaponHitBox; }
}
