using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text weaponNameText;

    [SerializeField] private PlayerFire player;

    private void OnEnable()
    {
        player.OnWeaponChanged += RefreshUI;
    }

    private void OnDisable()
    {
        player.OnWeaponChanged -= RefreshUI;
    }

    private void RefreshUI(string weaponName, Sprite weaponIcon)
    {
        weaponNameText.text = weaponName;
        icon.sprite = weaponIcon;
    }
}
