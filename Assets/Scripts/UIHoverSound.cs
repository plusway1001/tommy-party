using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class UIHoverSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            AudioManager.Instance.PlaySFX(hoverSound);
        }
    }
}