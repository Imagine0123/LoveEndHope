using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement & Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundLayer;

    [Header("References")]
    public PickUpManager pickUpManager;
    public ProjectileLaunch projectileLaunch;
    public PauseMenu pauseMenu;
    public PlayerHealth playerHealth;
    public GameObject tutorialScreen;
    public Transform muzzleTransform;

    [Header("Knockback")]
    public float KnockbackForce;
    public float KnockbackCounter;
    public float KnockbackLength;
    public bool KnockFromRight;
    
    // Components
    private Rigidbody2D Body;
    private SpriteRenderer sprite;
    private Animator animator;
    private BoxCollider2D boxCollider;

    // State Machine
    private enum PlayerState { Idle, Running, Jumping, Falling, Melee, Shooting, Reloading }
    private PlayerState state;
    
    // Private State Variables
    private float horizontalInput;
    private int JumpCount;
    private int JumpLimit = 2;
    public bool facingRight = true;
    private bool isReloading;
    private float reloadTimer;
    private bool isShooting;
    private bool isMeleeing;
    private float meleeTimer;
    private float stepTimer;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!pauseMenu.isPaused)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }
        else
        {
            horizontalInput = 0;
        }
        
        HandleTutorial();
        HandleFallDeath();

        if (!pauseMenu.isPaused)
        {
            HandleJumping();
            HandleReloading();
            HandleShooting();
            HandleMelee();
        }

        HandleReloadTimer();
        HandleMeleeTimer();
        
        FlipSpriteBasedOnMovement();
        HandleFootsteps();
        UpdateState();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.isPaused)
                pauseMenu.Resume();
            else
                pauseMenu.Pause();
        }
    }

    private void FixedUpdate()
    {
        if (KnockbackCounter <= 0 && !isMeleeing)
        {
            Body.linearVelocity = new Vector2(horizontalInput * speed, Body.linearVelocity.y);
        }
        else if (KnockbackCounter > 0)
        {
            HandleKnockback();
        }
        else if (isMeleeing)
        {
            Body.linearVelocity = new Vector2(0, Body.linearVelocity.y);
        }
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && JumpCount < JumpLimit)
        {
            Body.linearVelocity = new Vector2(Body.linearVelocity.x, jumpHeight);
            JumpCount++;
        }
    }

    private void HandleReloading()
    {
        if (Input.GetKeyDown(KeyCode.R) && pickUpManager.hasPistol && !isReloading)
        {
            isReloading = true;
            reloadTimer = 1.65f;
            SoundManager.instance.PlaySound2D("PistolReload");
        }
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && pickUpManager.hasPistol && projectileLaunch.currentClip > 0 && !isReloading)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }
    }

    private void HandleMelee()
    {
        if (Input.GetKey(KeyCode.F) && !isMeleeing)
        {
            isMeleeing = true;
            meleeTimer = 0.4f;
        }
    }
    
    private void HandleReloadTimer()
    {
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                isReloading = false;
                projectileLaunch.Reload();
            }
        }
    }

    private void HandleMeleeTimer()
    {
        if (isMeleeing)
        {
            meleeTimer -= Time.deltaTime;
            if (meleeTimer <= 0)
            {
                isMeleeing = false;
            }
        }
    }

    private void HandleKnockback()
    {
        if (KnockFromRight)
            Body.linearVelocity = new Vector2(-KnockbackForce, KnockbackForce);
        else
            Body.linearVelocity = new Vector2(KnockbackForce, KnockbackForce);

        KnockbackCounter -= Time.deltaTime;
    }

    private void FlipSpriteBasedOnMovement()
    {
        if (isShooting || isMeleeing) return;
        
        if (horizontalInput > 0.1f)
        {
            sprite.flipX = false;
            facingRight = true;
            muzzleTransform.localPosition = new Vector3(Mathf.Abs(muzzleTransform.localPosition.x), muzzleTransform.localPosition.y, muzzleTransform.localPosition.z);
        }
        else if (horizontalInput < -0.1f)
        {
            sprite.flipX = true;
            facingRight = false;
            muzzleTransform.localPosition = new Vector3(-Mathf.Abs(muzzleTransform.localPosition.x), muzzleTransform.localPosition.y, muzzleTransform.localPosition.z);
        }
    }
    
    public void ForceFlipTowards(Vector3 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            sprite.flipX = true;
            facingRight = false;
            muzzleTransform.localPosition = new Vector3(-Mathf.Abs(muzzleTransform.localPosition.x), muzzleTransform.localPosition.y, muzzleTransform.localPosition.z);
        }
        else if (targetPosition.x > transform.position.x)
        {
            sprite.flipX = false;
            facingRight = true;
            muzzleTransform.localPosition = new Vector3(Mathf.Abs(muzzleTransform.localPosition.x), muzzleTransform.localPosition.y, muzzleTransform.localPosition.z);
        }
    }

    private void HandleFootsteps()
    {
        if (stepTimer > 0)
        {
            stepTimer -= Time.deltaTime;
        }
        else if (Mathf.Abs(Body.linearVelocity.x) > 0.1f && IsGrounded() && !isReloading && !isShooting && !isMeleeing)
        {
            SoundManager.instance.PlaySound3D("PlayerFootstep", transform.position);
            stepTimer = 0.5f;
        }
    }

    private void UpdateState()
    {
        if (isMeleeing)
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
        else if (Body.linearVelocity.y > 0.1f)
        {
            state = PlayerState.Jumping;
        }
        else if (Body.linearVelocity.y < -0.1f)
        {
            state = PlayerState.Falling;
        }
        else if (Mathf.Abs(horizontalInput) > 0)
        {
            state = PlayerState.Running;
        }
        else
        {
            state = PlayerState.Idle;
        }

        animator.SetInteger("State", (int)state);
    }
    
    private void HandleTutorial()
    {
        if (tutorialScreen != null && tutorialScreen.activeInHierarchy && Input.GetButtonDown("Horizontal"))
        {
            tutorialScreen.SetActive(false);
        }
    }
    
    private void HandleFallDeath()
    {
        if (transform.position.y <= -20f)
        {
            playerHealth.takeDamage(9999);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            JumpCount = 0;
        }
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.down,
            extraHeight,
            groundLayer
        );

        return raycastHit.collider != null;
        }
}