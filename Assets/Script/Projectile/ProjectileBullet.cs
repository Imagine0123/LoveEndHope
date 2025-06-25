using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    public Rigidbody2D projRigidBody;
    public float speed;
    private ZombieHealth zombieHealth;
    public int damage;

    public float projLife;
    public float projCount;

    public PlayerMovement playerMovement;
    public bool facingRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projCount = projLife;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        facingRight = playerMovement.facingRight;
        if (!facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        projCount -= Time.deltaTime;
        if (projCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (facingRight)
        {
            projRigidBody.linearVelocity = new Vector2(speed, projRigidBody.linearVelocityY);
        }
        else
        {
            projRigidBody.linearVelocity = new Vector2(-speed, projRigidBody.linearVelocityY);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            if (zombieHealth == null)
            {
                zombieHealth = collision.gameObject.GetComponent<ZombieHealth>();
            }
            SoundManager.instance.PlaySound2D("BulletHitFlesh");
            zombieHealth.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
