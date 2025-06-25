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
    public PauseMenu pauseMenu;
    public PlayerHealth playerHealth;
    public GameObject tutorialScreen;

    public float KnockbackForce;
    public float KnockbackCounter;
    public float KnockbackLength;
    public bool KnockFromRight;
    public bool flipLeft;
    public bool facingRight;

    private float reloadTimer;
    private bool isReloading;
    private float stepTimer;
    private bool isShooting;
    private float meleeTimer;
    private bool isMeleeing;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (tutorialScreen == isActiveAndEnabled)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                tutorialScreen.SetActive(false);
            }
        }
        if (transform.position.y <= -20f)
        {
            playerHealth.takeDamage(9999);
        }

        if (KnockbackCounter <= 0 && !isReloading && !isShooting && !isMeleeing)
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
        else if (Input.GetKeyDown(KeyCode.R) && pickUpManager.hasPistol && pauseMenu.isPaused == false)
        {
            isReloading = true;
            reloadTimer = 1.65f;
            SoundManager.instance.PlaySound2D("PistolReload");
        }

        if (Input.GetButton("Fire1") && pickUpManager.hasPistol && projectileLaunch.currentClip > 0 && pauseMenu.isPaused == false)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }

        if (isMeleeing)
        {
            meleeTimer -= Time.deltaTime;
            if (meleeTimer <= 0)
            {
                isMeleeing = false;
            }
        }
        else if (Input.GetKey(KeyCode.F) && pauseMenu.isPaused == false)
        {
            isMeleeing = true;
            meleeTimer = 0.4f;
        }

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

        if (stepTimer > 0)
        {
            stepTimer -= Time.deltaTime;
        }
        else if (Body.linearVelocity.x != 0f && !isReloading && !isShooting)
        {
            SoundManager.instance.PlaySound3D("PlayerFootstep", transform.position);
            stepTimer = 0.5f;
        }

        UpdateState();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.isPaused)
                pauseMenu.Resume();
            else
                pauseMenu.Pause();
        }
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
        else if (isMeleeing)
        {
            state = PlayerState.Melee;
        }
        else if (isShooting)
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

