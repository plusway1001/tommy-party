using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Hit Flash")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Health")]
    public int maxHealth = 3;

    private int currentHealth;

    private Rigidbody2D rb;

    public float knockbackForce = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;

        // Flash effect
        StartCoroutine(HitFlash());

        rb.AddForce(hitDirection * knockbackForce,
            ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator HitFlash()
    {
        // Change color
        spriteRenderer.color = flashColor;

        // Wait
        yield return new WaitForSeconds(flashDuration);

        // Back to normal
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
