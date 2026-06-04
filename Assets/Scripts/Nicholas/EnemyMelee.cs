using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private GameObject lootPrefab;

    private Enemy enemy;
    private Health health;
    private Transform player;

    private bool detected = false;

    private float nextFireTime;

    private Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        enemy = EnemiesList.melee;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        health = GetComponent<Health>();
        health.Initialize(enemy.health);
        health.OnDeath += DropLoot;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= enemy.detectionRange)
        {
            detected = true;
        }

        moveDirection = Vector2.zero;

        if (distance > enemy.stoppingRange)
        {
            moveDirection = direction.normalized * enemy.speed;
        }

        if (detected)
        {
            FacePlayer(direction);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
    }

    private void FacePlayer(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float newAngle = Mathf.LerpAngle(rb.rotation, angle - 90f, enemy.turnSpeed * Time.deltaTime);

        rb.MoveRotation(newAngle);
    }

    private void DropLoot()
    {
        foreach (Loot loot in enemy.lootTable)
        {
            if (Random.value <= loot.dropChance)
            {
                GameObject drop = Instantiate(lootPrefab, transform.position, Quaternion.identity);
                drop.GetComponent<LootPickup>().loot = loot;
            }
        }
    }
}
