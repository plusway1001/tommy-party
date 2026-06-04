using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Health health;
    [SerializeField] private int maxHealth;
    [SerializeField] public float iFrameDuration;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
        health.Initialize(maxHealth);
    }
}
