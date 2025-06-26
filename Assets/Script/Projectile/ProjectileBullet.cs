using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    public Rigidbody2D projRigidBody;
    public float speed;
    public int damage;
    public float projLife;
    public float projCount;
    
    private ZombieHealth zombieHealth;

    void Start()
    {
        projCount = projLife;
    }

    void Update()
    {
        projCount -= Time.deltaTime;
        if (projCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            SoundManager.instance.PlaySound2D("BulletHitFlesh");

            zombieHealth = collision.gameObject.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.takeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}