using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    private Weapon currentWeapon;
    private float nextFireTime;

    private void Start()
    {
        currentWeapon = WeaponsList.starter;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if(Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + (1f / currentWeapon.fireRate);

        Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
