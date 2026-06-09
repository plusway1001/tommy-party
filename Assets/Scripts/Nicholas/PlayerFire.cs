using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    private float nextFireTime;

    private int value;
    private int damage;
    private float fireRate;
    private string prefabName;

    [SerializeField] private GameObject starterBulletPrefab;

    private Dictionary<string, GameObject> prefabDictionary;

    private void Start()
    {
        prefabDictionary = new Dictionary<string, GameObject>()
        {
            { "bullet_starter", starterBulletPrefab }
        };

        LoadExcelData();
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

        nextFireTime = Time.time + (1f / fireRate);

        if (prefabDictionary.TryGetValue(prefabName, out GameObject selectedPrefab))
        {
            GameObject bullet = Instantiate(selectedPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().damage = damage;
        }   
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

            if (columns[0] != "0") continue;

            value = int.Parse(columns[2]);
            damage = int.Parse(columns[3]);
            fireRate = float.Parse(columns[4]);
            prefabName = columns[5];
        }
    }
}
