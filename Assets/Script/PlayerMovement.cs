using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    private Rigidbody2D Body;
    private int JumpCount;
    private int JumpLimit = 2;
    private SpriteRenderer sprite;
    private enum PlayerState { Idle, Running, Jumping, Falling, Melee, Shooting } // 0 1 2 3 4 5
    private PlayerState state;
    private Animator animator;
    public PistolManager pistolManager;

    public float KnockbackForce;
    public float KnockbackCounter;
    public float KnockbackLength;
    public bool KnockFromRight;


    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (KnockbackCounter <= 0)
        {
            Body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, Body.linearVelocity.y);
        }
        else
        {
            if (KnockFromRight == true)
            {
                Body.linearVelocity = new Vector2(-KnockbackForce, KnockbackForce);
            }
            if (KnockFromRight == false)
            {
                Body.linearVelocity = new Vector2(KnockbackForce, KnockbackForce);
            }
            KnockbackCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && JumpCount < JumpLimit)
        {
            Body.linearVelocity = new Vector2(Body.linearVelocity.x, jumpHeight);
            JumpCount++;
        }

        FlipSprite();
        UpdateState();
    }

    private void FlipSprite()
    {
        if (Body.linearVelocity.x > 0.1f)
        {
            sprite.flipX = false;
        }
        else if (Body.linearVelocity.x < -0.1f)
        {
            sprite.flipX = true;
        }
    }

    private void UpdateState()
    {
        if (Body.linearVelocity.y > .1f)
        {
            state = PlayerState.Jumping;
        }
        else if (Body.linearVelocity.x != 0f)
        {
            state = PlayerState.Running;
        }
        else if (Body.linearVelocity.y < -.1f)
        {
            state = PlayerState.Falling;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            state = PlayerState.Melee;
        }

        else if (Input.GetKey(KeyCode.Z) && pistolManager.hasPistol)
        {
            state = PlayerState.Shooting;
        }

        else
        {
            state = PlayerState.Idle;
        }

        animator.SetInteger("State", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            JumpCount = 0;
        }
    }
}