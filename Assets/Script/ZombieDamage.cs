using UnityEngine;

public class ZombieDamage : MonoBehaviour
{
    public int damage;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerHealth == null)
            {
                playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            }
            if (playerMovement == null)
            {
                playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            }
            playerMovement.KnockbackCounter = playerMovement.KnockbackLength;
            if (collision.transform.position.x <= transform.position.x)
            {
                playerMovement.KnockFromRight = true;
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.KnockFromRight = false;
            }
            playerHealth.takeDamage(damage);
        }
    }
}
