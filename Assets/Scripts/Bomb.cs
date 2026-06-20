using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float fuseTime = 2f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage = 100;

    [SerializeField] private GameObject explosionPrefab;
    private void Start()
    {
        Invoke("Explode", fuseTime);
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            EnemyBehaviour enemy = hit.GetComponent<EnemyBehaviour>();

            if (enemy != null)
            {
                enemy.GetComponent<Health>().TakeDamage(damage);
            }
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}