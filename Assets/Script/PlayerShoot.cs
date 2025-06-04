using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Firing Settings")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float fireRate = 0.3f; // seconds between shots

    private float lastFireTime = 0f;

    void Update()
    {
        // Only allow fire if the player actually has the pistol:
        if (PistolManager.instance != null && PistolManager.instance.hasPistol)
        {
            if (Input.GetKey(KeyCode.Z) && Time.time - lastFireTime > fireRate)
            {
                FireBullet();
                lastFireTime = Time.time;
            }
        }
        // If the player does NOT have a pistol, Fire1 does nothing.
    }

    private void FireBullet()
    {
        // Instantiate the bullet prefab at the muzzle point.
        if (bulletPrefab != null && muzzlePoint != null)
        {
            Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, null);
            // (Optionally, play muzzle flash, sound, etc.)
        }
        else
        {
            Debug.LogWarning("[PlayerShoot] Missing bulletPrefab or muzzlePoint!");
        }
    }
}