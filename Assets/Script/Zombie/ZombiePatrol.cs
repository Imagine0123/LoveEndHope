using UnityEngine;

public class ZombiePatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;
    public float stepTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("IsMoving", true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0f);
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else if (rb.linearVelocity.x != 0f)
            {
                SoundManager.instance.PlaySound3D("ZombieFootstep", transform.position);
                stepTimer = 0.8f;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0f);
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else if (rb.linearVelocity.x != 0f)
            {
                SoundManager.instance.PlaySound3D("ZombieFootstep", transform.position);
                stepTimer = 0.8f;
            }
            
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}