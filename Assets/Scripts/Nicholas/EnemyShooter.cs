using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private string enemyID = "0";

    private int baseHealth;
    private float baseSpeed;
    private float fireRate;
    private float turnSpeed;
    private float detectionRange;
    private float stoppingRange;

    private string lootTableID;

    private Health health;

    private Rigidbody2D rb;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject lootPrefab;

    private Transform player;

    private bool detected = false;

    private float nextFireTime;

    private Vector2 moveDirection;

    private LootTable lootTable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        lootTable = new LootTable();
        LoadCSV();
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
        if (Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + (1f / fireRate);

        if (firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private void DropLoot()
    {
        if (lootTable == null || lootTable.items.Count == 0) return;

        bool itemDropped = false;

        if (!itemDropped)
        {
            foreach (LootItem loot in lootTable.items)
            {
                if (Random.value <= loot.dropChance)
                {
                    GameObject drop = Instantiate(lootPrefab, transform.position, Quaternion.identity);
                    //drop.GetComponent<LootPickup>().loot = loot;
                    itemDropped = true;
                }
            }
        }
    }

    void LoadCSV()
    {
        //Loading CSV file from the Resources folder
        TextAsset enemyCSV = Resources.Load<TextAsset>("EnemyList");
        TextAsset lootCSV = Resources.Load<TextAsset>("LootList");

        if (enemyCSV == null)
        {
            return;
        }

        string[] enemyRows = enemyCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < enemyRows.Length; i++)
        {
            //Skip empty rows
            if (string.IsNullOrWhiteSpace(enemyRows[i])) continue;

            //Split columns by comma delimiter
            string[] columns = enemyRows[i].Split(',');

            if (columns[0] != enemyID) continue;

            baseHealth = int.Parse(columns[2]);
            baseSpeed = float.Parse(columns[3]);

            fireRate = float.Parse(columns[4]);
            turnSpeed = float.Parse(columns[5]);
            detectionRange = float.Parse(columns[6]);
            stoppingRange = float.Parse(columns[7]);

            lootTableID = columns[8];
        }

        if (lootCSV == null)
        {
            return;
        }

        string[] lootRows = enemyCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < lootRows.Length; i++)
        {
            //Skip empty rows
            if (string.IsNullOrWhiteSpace(lootRows[i])) continue;

            //Split columns by comma delimiter
            string[] columns = lootRows[i].Split(',');

            LootItem item = new LootItem
            {
                name = columns[1],
                value = int.Parse(columns[2]),
                dropChance = float.Parse(columns[3]),
                icon = columns[4]
            };

            lootTable.items.Add(item);
        }
    }
}
