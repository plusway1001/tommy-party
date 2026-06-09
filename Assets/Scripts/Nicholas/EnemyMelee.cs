using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    private string enemyID = "1";

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        LoadExcelData();
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
        //foreach (Loot loot in enemy.lootTable)
        //{
        //    if (Random.value <= loot.dropChance)
        //    {
        //        GameObject drop = Instantiate(lootPrefab, transform.position, Quaternion.identity);
        //        drop.GetComponent<LootPickup>().loot = loot;
        //    }
        //}
    }

    void LoadExcelData()
    {
        //Loading CSV file from the Resources folder
        TextAsset enemyCSV = Resources.Load<TextAsset>("EnemyList");

        if (enemyCSV == null)
        {
            return;
        }

        string[] rows = enemyCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < rows.Length; i++)
        {
            //Skip empty rows
            if (string.IsNullOrWhiteSpace(rows[i])) continue;

            //Split columns by comma delimiter
            string[] columns = rows[i].Split(',');

            if (columns[0] != enemyID) continue;

            baseHealth = int.Parse(columns[2]);
            baseSpeed = float.Parse(columns[3]);

            fireRate = float.Parse(columns[4]);
            turnSpeed = float.Parse(columns[5]);
            detectionRange = float.Parse(columns[6]);
            stoppingRange = float.Parse(columns[7]);

            lootTableID = columns[8];
        }
    }
}
