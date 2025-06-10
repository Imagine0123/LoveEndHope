using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player
    public float smoothSpeed = 0.125f;
    public float horizontalOffset = 3f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        // Arah mouse dibandingkan ke posisi pemain
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float direction = Mathf.Sign(mouseWorldPos.x - target.position.x);

        // Hitung posisi target dengan offset ke arah mouse
        Vector3 desiredPosition = new Vector3(
            target.position.x + (horizontalOffset * direction),
            target.position.y,
            transform.position.z // tetap jaga z
        );

        // Smooth camera movement
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
