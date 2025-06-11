using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    private Rigidbody2D Body;
    private int JumpCount;
    private int JumpLimit = 2;
    private SpriteRenderer sprite;
    private enum PlayerState { Idle, Running, Jumping, Falling, Melee, Shooting, Reloading } // 0 1 2 3 4 5 6
    private PlayerState state;
    private Animator animator;
    public PickUpManager pickUpManager;
    public ProjectileLaunch projectileLaunch;

    public float KnockbackForce;
    public float KnockbackCounter;
    public float KnockbackLength;
    public bool KnockFromRight;
    public bool flipLeft;
    public bool facingRight;

    private float reloadTimer;
    private bool isReloading;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (KnockbackCounter <= 0 && !isReloading)
        {
            Body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, Body.linearVelocity.y);
        }
        else if (KnockbackCounter > 0)
        {
            if (KnockFromRight)
            {
                Body.linearVelocity = new Vector2(-KnockbackForce, KnockbackForce);
            }
            else
            {
                Body.linearVelocity = new Vector2(KnockbackForce, KnockbackForce);
            }
            KnockbackCounter -= Time.deltaTime;
        }

        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                isReloading = false;
                projectileLaunch.Reload();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && JumpCount < JumpLimit)
            {
                Body.linearVelocity = new Vector2(Body.linearVelocity.x, jumpHeight);
                JumpCount++;
            }

            if (Body.linearVelocity.x > 0.1f)
            {
                facingRight = true;
                FlipSprite(true);
            }
            else if (Body.linearVelocity.x < -0.1f)
            {
                facingRight = false;
                FlipSprite(false);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                isReloading = true;
                reloadTimer = 2.15f;
            }
        }

        UpdateState();
    }

    private void FlipSprite(bool facingRight)
    {
        if (flipLeft && facingRight)
        {
            transform.Rotate(0f, -180f, 0f);
            flipLeft = false;
        }
        else if (!flipLeft && !facingRight)
        {
            transform.Rotate(0f, -180f, 0f);
            flipLeft = true;
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
        else if (Input.GetButton("Fire1") && pickUpManager.hasPistol && projectileLaunch.currentClip > 0)
        {
            state = PlayerState.Shooting;
        }
        else if (isReloading)
        {
            state = PlayerState.Reloading;
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
