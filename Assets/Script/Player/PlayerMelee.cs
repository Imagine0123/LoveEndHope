using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;
    public LayerMask enemyMask;

    public int attackDamage;
    public float knockbackForce = 10f;

    public float cooldownTime;
    private float cooldownTimer = 0f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }
    // Update is called once per frame
    private void Update()
    {
        if (cooldownTimer >= 0)
        {
            if (Input.GetKey(KeyCode.F))
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<ZombieHealth>().takeDamage(attackDamage);

                    Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 knockbackDirection = (enemy.transform.position - attackOrigin.position).normalized;
                        rb.linearVelocity = knockbackDirection * knockbackForce;
                    }
                }
                cooldownTimer = cooldownTime;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}

