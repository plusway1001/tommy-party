using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    [Header("Chase")]
    public float moveSpeed = 3f;

    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        // Direction to player
        Vector2 direction =
            (player.position - transform.position).normalized;

        // Move toward player
        rb.MovePosition(
            rb.position + direction * moveSpeed * Time.fixedDeltaTime
        );

        // Rotate toward player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rb.rotation = angle;
    }
}
