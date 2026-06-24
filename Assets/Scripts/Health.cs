using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum flashtype
{
    Color,
    Material
}

public class Health : MonoBehaviour
{
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    public bool invincible;

    public Action OnDeath;

    [Header("Hit Flash")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    [SerializeField] private Material flashMaterial;
    [SerializeField] private Material originalMaterial;

    public flashtype flashtype;

    private SpriteRenderer sr;
    private Color originalColor;

    public bool dead = false;

    EnemySpawner spawner;

    [SerializeField] private GameObject MultiplyEnemies;
    //[SerializeField] private int enemyIDMultiply;
    [SerializeField] private bool canMultiply;
    [SerializeField] private float spawnDistance = 5f;

    //[SerializeField] private AudioClip hitSound;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        spawner = FindFirstObjectByType<EnemySpawner>();

        if (flashtype == flashtype.Color)
        {
            originalColor = sr.color;
        }
        else
        {
            originalMaterial = sr.material;
        }
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

        if (sr == null) return;

        if (flashtype == flashtype.Color)
        {
            sr.color = originalColor;
        }
        else
        {
            sr.material = originalMaterial;
        }
    }

    public void TakeDamage(int damage)
    {
        // Flash effect
        CameraShake.Instance.Shake(0.1f, 0.05f);
        if (gameObject.CompareTag("Player"))
        {
            if (!invincible)
            {
                currentHealth -= damage;
                currentHealth = Mathf.Max(currentHealth, 0);
                ParticleEffectManager.Instance.PlayHitEffect(transform.position);
                //AudioManager.Instance.PlaySFX(hitSound);
                if (flashtype == flashtype.Color)
                {
                    StartCoroutine(HitFlashColor());
                }
                else
                {
                    StartCoroutine(HitFlashMat());
                }

                invincible = true;

                StartCoroutine(Invincibility(GetComponent<PlayerHealth>().iFrameDuration));
            }
        }
        else
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);
            //StartCoroutine(HitFlashColor());
            if (flashtype == flashtype.Color)
            {
                StartCoroutine(HitFlashColor());
            }
            else
            {
                StartCoroutine(HitFlashMat());
            }
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

    IEnumerator HitFlashColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Color flash = new Color(1f, 0f, 0f, sr.color.a);

        sr.color = flash;

        yield return new WaitForSeconds(flashDuration);

        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, sr.color.a);
    }

    IEnumerator HitFlashMat()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.material = flashMaterial;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMaterial;
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
            if (!(enemy.multiplyMinCount == 0 || enemy.multiplyMaxCount == 0))
            {
                MultiplyEnemiesSpawn();
            }
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

    private void MultiplyEnemiesSpawn()
    {
        EnemyBehaviour enemyIDID = GetComponent<EnemyBehaviour>();
        int SpawnCount = UnityEngine.Random.Range(enemyIDID.multiplyMinCount, enemyIDID.multiplyMaxCount);

        if (enemyIDID != null)
        {
            for (int a = 0; a < SpawnCount; a++)
            {
                float xOffset = UnityEngine.Random.value > 0.5f ? spawnDistance : -spawnDistance;

                Vector2 spawnPosition = new Vector2(transform.position.x + xOffset, transform.position.y);
                GameObject enemyObject = Instantiate(MultiplyEnemies, spawnPosition, Quaternion.identity);
                EnemyBehaviour enemy = enemyObject.GetComponent<EnemyBehaviour>();
                enemy.enemyID = enemyIDID.enemyID;
                enemy.InitializeEnemy();

                enemy.multiplyMinCount = 0;
                enemy.multiplyMaxCount = 0;

                enemy.contactDamage /= 2;

                spawner.aliveEnemies++;
            }
        }
    }
}
