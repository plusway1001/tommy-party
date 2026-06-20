using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    private float nextFireTime;

    public float fireRatePowerUp = 0f;

    private WeaponList weaponList;
    private Weapon currentWeapon;
    [SerializeField] private int currentWeaponID;

    private void Awake()
    {
        weaponList = new WeaponList();
    }

    private void Start()
    {
        LoadExcelData();
        currentWeapon = weaponList.weapons[currentWeaponID];
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

        nextFireTime = Time.time + (1f / currentWeapon.fireRate) - fireRatePowerUp;

        GameObject bullet = Instantiate(Resources.Load<GameObject>($"Prefabs/Bullets/{currentWeapon.prefabName}"), firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = currentWeapon.damage;
        bulletScript.originTag = gameObject.tag;
    }

    void LoadExcelData()
    {
        //Loading CSV file from the Resources folder
        TextAsset weaponCSV = Resources.Load<TextAsset>("WeaponList");

        if (weaponCSV == null)
        {
            return;
        }

        string[] rows = weaponCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < rows.Length; i++)
        {
            //Skip empty rows
            if (string.IsNullOrWhiteSpace(rows[i])) continue;

            //Split columns by comma delimiter
            string[] columns = rows[i].Split(',');

            Weapon weapon = new Weapon
            {
                value = int.Parse(columns[2]),
                damage = int.Parse(columns[3]),
                fireRate = float.Parse(columns[4]),
                prefabName = columns[5]
            };

            weaponList.weapons.Add(weapon);
        }
    }
}
