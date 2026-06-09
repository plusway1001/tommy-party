using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (Time.time - spawnTime < 0.05f)
        {
            return;
        }

        if (health != null)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
