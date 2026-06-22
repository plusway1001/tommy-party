using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Weapon
{
    public string name;
    public int value;
    public int damage;
    public float fireRate;
    public string prefabName;

    public Sprite sprite;
}

public class WeaponList
{
    public List<Weapon> weapons = new List<Weapon>();
}
