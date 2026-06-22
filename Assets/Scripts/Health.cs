using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    public bool invincible;

    public Action OnDeath;

    [Header("Hit Flash")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer sr;
    private Color originalColor;

    public bool dead = false;

    EnemySpawner spawner;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        spawner = FindFirstObjectByType<EnemySpawner>();

        originalColor = sr.color;
    }

    private void Start()
    {
        dead = false;
        invincible = false;
        SetAlpha(1.0f);

        if (gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        }
    }

    public void Initialize(int health)
    {
        maxHealth = health;
        currentHealth = health;

        sr.color = originalColor;
    }

    public void TakeDamage(int damage)
    {
        // Flash effect
        //CameraShake.Instance.Shake(0.1f, 0.15f);
        if (gameObject.CompareTag("Player"))
        {
            if (!invincible)
            {
                currentHealth -= damage;
                currentHealth = Mathf.Max(currentHealth, 0);
                ParticleEffectManager.Instance.PlayHitEffect(transform.position);
                StartCoroutine(HitFlash());

                invincible = true;

                StartCoroutine(Invincibility(GetComponent<PlayerHealth>().iFrameDuration));
            }
        }
        else
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);
            StartCoroutine(HitFlash());
        }

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator Invincibility(float duration)
    {
        SetAlpha(0.1f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        yield return new WaitForSeconds(duration);

        SetAlpha(1.0f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        invincible = false;
    }

    IEnumerator HitFlash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Color flash = new Color(1f, 0f, 0f, sr.color.a);

        sr.color = flash;

        yield return new WaitForSeconds(flashDuration);

        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, sr.color.a);
    }

    private void SetAlpha(float a)
    {
        Color c = sr.color;
        c.a = a;
        sr.color = c;

        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            Color cc = s.color;
            cc.a = a;
            s.color = cc;
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        ParticleEffectManager.Instance.PlayExplosionEffect(transform.position);

        if (gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = gameObject.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                spawner.OnEnemyKilled(enemy.enemyID);
            }
        }
        
        else if (gameObject.CompareTag("Player"))
        {
            dead = true;
        }

        Destroy(gameObject);
    }
}
