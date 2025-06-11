using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public ProjectileLaunch projectileLaunch;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            projectileLaunch.AddAmmo(20);
            Destroy(gameObject);
        }
    }
}
