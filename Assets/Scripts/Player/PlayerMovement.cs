using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Camera cam;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float acceleration;

    public Vector3 initialPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Start()
    {
        initialPos = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, movementInput * movementSpeed, acceleration * Time.fixedDeltaTime);

        Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePos - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    public void Knockback(Transform enemy, float force)
    {
        if (rb != null)
        {
            Vector2 knockbackDirection = (transform.position - enemy.position).normalized;

            rb.AddForce(knockbackDirection * force, ForceMode2D.Impulse);
        }
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        gameObject.transform.position = initialPos;
    }
}
