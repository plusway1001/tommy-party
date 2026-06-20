using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    [HideInInspector] public string originTag;

    private void Start()
    {
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

        else if (collision.gameObject.CompareTag(originTag))
        {
            return;
        }

        else if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

}
