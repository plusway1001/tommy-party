using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    private bool invincible;

    public Action OnDeath;

    [Header("Hit Flash")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Initialize(int health)
    {
        maxHealth = health;
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        // Flash effect
        CameraShake.Instance.Shake(0.1f, 0.15f);
        StartCoroutine(HitFlash());
        if (GetComponent<PlayerHealth>() != null)
        {
            if (!invincible)
            {
                currentHealth -= damage;
                invincible = true;
                foreach (SpriteRenderer sprite in GetComponentsInParent<SpriteRenderer>())
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);
                }
                foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);  
                }
                StartCoroutine(Invincibility(GetComponent<PlayerHealth>().iFrameDuration));
            }
        }
        else
        {
            currentHealth -= damage;
        }

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator Invincibility(float duration)
    {
        yield return new WaitForSeconds(duration);
        invincible = false;
        foreach (SpriteRenderer sprite in GetComponentsInParent<SpriteRenderer>())
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
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

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
