using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    public static ParticleEffectManager Instance;

    [Header("Particle Prefabs")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject healEffect;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Generic Spawn Function
    public void PlayEffect(GameObject effectPrefab, Vector2 position)
    {
        GameObject effect = Instantiate(effectPrefab, position, Quaternion.identity);

        // Auto destroy after particle duration
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(effect, 3f);
        }
    }

    // Specific Functions
    public void PlayHitEffect(Vector2 position)
    {
        PlayEffect(hitEffect, position);
    }

    public void PlayExplosionEffect(Vector2 position)
    {
        PlayEffect(explosionEffect, position);
    }

    public void PlayHealEffect(Vector2 position)
    {
        PlayEffect(healEffect, position);
    }
}
