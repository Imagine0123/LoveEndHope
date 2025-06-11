using TMPro;
using UnityEngine;

public class AmmoText : MonoBehaviour
{
    public ProjectileLaunch projectileLaunch;
    public TextMeshProUGUI ammoText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoText();
    }
    
    public void UpdateAmmoText()
    {
        ammoText.text = $"{projectileLaunch.currentClip} / {projectileLaunch.currentAmmo}";
    }
}
