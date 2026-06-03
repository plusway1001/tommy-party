using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float speed = 15f;
    public float lifeTime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = transform.up * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore player collision
        if (collision.CompareTag("Player"))
            return;

        Destroy(gameObject);
    }
}

