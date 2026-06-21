using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Health health;
    [SerializeField] public int maxHealth;
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
