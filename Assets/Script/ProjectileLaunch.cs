using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;

    public float shootTime;
    public float shootCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootCount = shootTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && shootCount <= 0 && PistolManager.instance.hasPistol)
        {
            Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
            shootCount = shootTime;
        }
        shootCount -= Time.deltaTime;
    }
}
