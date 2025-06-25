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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootCount = shootTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && shootCount <= 0 && PickUpManager.instance.hasPistol && currentClip > 0 && !pauseMenu.isPaused)
        {
            SoundManager.instance.PlaySound2D("PistolShoot");
            Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
            shootCount = shootTime;
            currentClip--;
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
