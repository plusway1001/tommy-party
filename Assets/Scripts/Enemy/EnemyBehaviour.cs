using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("0 = Shooter, 1 = Melee")]
    public int enemyID = 0;

    private int baseHealth;
    private float baseSpeed;
    private float fireRate;
    private float turnSpeed;
    private float detectionRange;
    private float stoppingRange;

    private int contactDamage;
    private float knockbackForce;

    private int lootTableID;

    private Health health;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private Transform player;

    private bool detected = false;

    private float nextFireTime;
    private Vector2 moveDirection;

    private LootTable lootTable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        lootTable = new LootTable();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        health = GetComponent<Health>();
        health.Initialize(baseHealth);
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

        if (distance <= detectionRange)
        {
            detected = true;
        }

        moveDirection = Vector2.zero;

        if (distance > stoppingRange)
        {
            moveDirection = direction.normalized * baseSpeed;
        }

        if (detected)
        {
            FacePlayer(direction);
            if (IsFacingPlayer(direction))
            {
                TryShoot();
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        Health player = collision.gameObject.GetComponent<Health>();

        if (!player.invincible)
        {
            player.TakeDamage(contactDamage);
            collision.gameObject.GetComponent<PlayerMovement>().Knockback(transform, knockbackForce);
        }
    }

    public void InitializeEnemy()
    {
        EnemyData data = EnemyDatabase.Instance.GetEnemy(enemyID);

        if (data == null)
        {
            return;
        }

        baseHealth = data.baseHealth;
        baseSpeed = data.baseSpeed;
        fireRate = data.fireRate;
        turnSpeed = data.turnSpeed;

        detectionRange = data.detectionRange;
        stoppingRange = data.stoppingRange;

        contactDamage = data.contactDamage;
        knockbackForce = data.knockbackForce;

        lootTableID = data.lootTableID;

        sr.sprite = data.sprite;

        lootTable = LootTableDatabase.Instance.GetTable(lootTableID);
    }

    private void FacePlayer(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float newAngle = Mathf.LerpAngle(rb.rotation, angle - 90f, turnSpeed * Time.deltaTime);

        rb.MoveRotation(newAngle);
    }

    private bool IsFacingPlayer(Vector2 direction)
    {
        Vector2 forward = transform.up;
        float angle = Vector2.Angle(forward, direction.normalized);

        return angle < 20f;
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime || enemyID == 1)
        {
            return;
        }

        nextFireTime = Time.time + (1f / fireRate);

        if (firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletPrefab.GetComponent<Bullet>().originTag = gameObject.tag;
        }
    }

    private void DropLoot()
    {
        if (lootTable.entries.Count == 0)
            return;

        float totalWeight = 0f;

        foreach (var entry in lootTable.entries)
        {
            totalWeight += entry.dropChance;
        }

        float roll = Random.Range(0f, totalWeight);

        foreach (var entry in lootTable.entries)
        {
            roll -= entry.dropChance;

            if (roll > 0)
            {
                continue;
            }

            LootData loot = LootDatabase.Instance.lootDatabase[entry.lootID];

            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Loot/{loot.prefab}");
            GameObject drop = Instantiate(prefab, transform.position, Quaternion.identity);

            LootPickup pickup = drop.GetComponent<LootPickup>();

            pickup.lootID = loot.lootID;

            break;
        }
    }
}
