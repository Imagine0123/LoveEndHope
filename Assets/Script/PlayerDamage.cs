using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int damage;
    private ZombieHealth zombieHealth;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (zombieHealth == null)
            {
                zombieHealth = collision.gameObject.GetComponent<ZombieHealth>();
            }
            zombieHealth.takeDamage(damage);
        }
    }
}
