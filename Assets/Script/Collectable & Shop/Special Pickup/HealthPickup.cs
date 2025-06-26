using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.takeDamage(-10);
            Destroy(gameObject);
        }
    }
}
