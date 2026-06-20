using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    public event Action<string, Sprite> OnWeaponChanged;

    [SerializeField] private Transform firePoint;

    private float nextFireTime;

    private WeaponList weaponList;
    private Weapon currentWeapon;
    [SerializeField] private int weaponID;

    private void Awake()
    {
        weaponList = new WeaponList();
    }

    private void Start()
    {
        LoadExcelData();
        currentWeapon = weaponList.weapons[weaponID];
        OnWeaponChanged?.Invoke(currentWeapon.name, currentWeapon.sprite);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Fire();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if(weaponID == weaponList.weapons.Count - 1)
            {
                weaponID = 0;
            }
            else
            {
                weaponID += 1;
            }

            currentWeapon = weaponList.weapons[weaponID];
            OnWeaponChanged?.Invoke(currentWeapon.name, currentWeapon.sprite);
        }
    }

    private void Fire()
    {
        if(Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + (1f / currentWeapon.fireRate);

        GameObject bullet = Instantiate(Resources.Load<GameObject>($"Prefabs/Bullets/{currentWeapon.prefabName}"), firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = currentWeapon.damage;
        bulletScript.originTag = gameObject.tag;
    }

    void LoadExcelData()
    {
        TextAsset weaponCSV = Resources.Load<TextAsset>("WeaponList");

        if (weaponCSV == null)
        {
            return;
        }

        string[] rows = weaponCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < rows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(rows[i])) continue;

            string[] columns = rows[i].Split(',');

            Weapon weapon = new Weapon
            {
                name = columns[1],
                value = int.Parse(columns[2]),
                damage = int.Parse(columns[3]),
                fireRate = float.Parse(columns[4]),
                prefabName = columns[5],
                sprite = Resources.Load<Sprite>($"Sprites/Weapons/{columns[6]}")
            };

            weaponList.weapons.Add(weapon);
        }
    }
}
