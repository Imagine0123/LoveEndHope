using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;
    public LayerMask enemyMask;

    public int attackDamage;
    public float stunDuration = 1f;

    private float cooldownTime = 0.4f;
    private float cooldownTimer = 0f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }

    private void Update()
    {
        if (cooldownTimer <= 0)
        {
            if (Input.GetKey(KeyCode.F))
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<ZombieHealth>().takeDamage(attackDamage);

                    ZombiePatrol patrol = enemy.GetComponent<ZombiePatrol>();
                    if (patrol != null)
                    {
                        patrol.Stun();
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

