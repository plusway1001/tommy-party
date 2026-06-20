using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPickup : MonoBehaviour
{
    public InventoryManager invent;

    public PlayerMovement PlayerMove;

    public PlayerFire FireRate;

    public Bullet damaged;

    public SpawnManager SpawnEnemy;

    public float AddPlayerMove = 1f, IncreasePlayerFireRate = 0.1f;
    public int IncreaseAttackDamaged = 1;

    public Transform[] SpawnPos;
    public GameObject[] EnemyType;

    private void OnTriggerStay2D(Collider2D others)
    {
        if (others.CompareTag("Item") && Keyboard.current.spaceKey.isPressed && InventoryManager.EnabledPickup)
        {
            invent.PlayerInventoryHit(others.gameObject);
        }

        if (others.CompareTag("PowerupMovement"))
        {
            PlayerMove.movementSpeed += AddPlayerMove;
            ParticleEffectManager.Instance.PlayExplosionEffect(transform.position);
            Destroy(others.gameObject);
        }

        if (others.CompareTag("PowerupFireRate"))
        {
            FireRate.fireRatePowerUp += IncreasePlayerFireRate;
            ParticleEffectManager.Instance.PlayExplosionEffect(transform.position);
            Destroy(others.gameObject);
        }

        if (others.CompareTag("PowerupAttackDamaged"))
        {
            damaged.damagedPowerUp += IncreaseAttackDamaged;
            ParticleEffectManager.Instance.PlayExplosionEffect(transform.position);
            Destroy(others.gameObject);
        }

        if (others.CompareTag("SpawnEnemyType1"))
        {
            SpawnEnemy.SpawnEnemyType(SpawnPos, EnemyType);
            Destroy(others.gameObject);
        }
    }
}
