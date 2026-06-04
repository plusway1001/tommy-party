using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /*[SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
    }*/

    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;

    // Camera shake offset
    [HideInInspector]
    public Vector3 shakeOffset;

    private void LateUpdate()
    {
        if (target != null)
        {
            // Follow player position
            Vector3 targetPos = new Vector3(
                target.position.x,
                target.position.y,
                transform.position.z
            );

            // Smooth follow
            Vector3 smoothPos = Vector3.Lerp(
                transform.position,
                targetPos,
                smoothSpeed * Time.deltaTime
            );

            // Add camera shake offset
            transform.position = smoothPos + shakeOffset;
        }
    }
}
