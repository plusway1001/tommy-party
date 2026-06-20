using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    [HideInInspector] public string originTag;

    public int damagedPowerUp = 0;

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
            if (collision.gameObject.CompareTag("Enemy"))
            {
                damage += damagedPowerUp;
            }
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
