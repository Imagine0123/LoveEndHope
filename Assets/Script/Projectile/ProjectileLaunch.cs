using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    [Header("Setup")]
    public GameObject bulletPrefab;
    public Transform muzzle;
    public PauseMenu pauseMenu;

    [Header("Timings & Cooldown")]
    public float shootTime;
    public float shootCount;
    public float animTimeMax;
    private float animTime;

    [Header("Ammo")]
    public int currentAmmo;
    public int maxAmmo;
    public int currentClip;
    public int maxClip;

    private PlayerMovement playerMovement;
    private bool isShooting;

    void Start()
    {
        shootCount = shootTime;
        playerMovement = GetComponentInParent<PlayerMovement>();
        isShooting = false;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && shootCount <= 0 && PickUpManager.instance.hasPistol && currentClip > 0 && !pauseMenu.isPaused && !isShooting)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerMovement.ForceFlipTowards(mouseWorldPosition);

            isShooting = true;
            animTime = 0;
        }

        if (isShooting)
        {
            animTime += Time.deltaTime;
            if (animTime >= animTimeMax)
            {
                SoundManager.instance.PlaySound2D("PistolShoot");

                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mouseWorldPosition - muzzle.position).normalized;
                
                GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                ProjectileBullet projBullet = bullet.GetComponent<ProjectileBullet>();
                if (projBullet != null)
                {
                    rb.linearVelocity = direction * projBullet.speed;
                }

                shootCount = shootTime;
                currentClip--;
                isShooting = false;
            }
        }

        if (shootCount > 0)
        {
            shootCount -= Time.deltaTime;
        }
    }

    public void Reload()
    {
        int reloadAmount = maxClip - currentClip;
        reloadAmount = (currentAmmo - reloadAmount) >= 0 ? reloadAmount : currentAmmo;
        currentAmmo -= reloadAmount;
        currentClip += reloadAmount;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }
}