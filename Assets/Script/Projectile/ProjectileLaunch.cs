using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;
    public PauseMenu pauseMenu;

    public float shootTime;
    public float shootCount;
    public int currentAmmo;
    public int maxAmmo;
    public int currentClip;
    public int maxClip;
    public float animTimeMax;
    private float animTime;
    private bool isShooting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootCount = shootTime;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && shootCount <= 0 && PickUpManager.instance.hasPistol && currentClip > 0 && !pauseMenu.isPaused && !isShooting)
        {
            animTime = 0;
            isShooting = true;
        }

        if (isShooting)
        {
            animTime += Time.deltaTime;
            if (animTime >= animTimeMax)
            {
                SoundManager.instance.PlaySound2D("PistolShoot");
                Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
                shootCount = shootTime;
                currentClip--;
                isShooting = false;
            }
        }

        shootCount -= Time.deltaTime;
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

