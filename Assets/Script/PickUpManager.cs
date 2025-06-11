using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager instance;
    public bool hasPistol = false;
    public GameObject pistolIcon;
    public GameObject knifeIcon;
    public GameObject AmmoCount;
    private void Awake()
    {
        // Classic singleton pattern (no “DontDestroy” for now, but you could add it if you need persistence across scenes).
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        // Every frame, update the UI text to reflect hasPistol:
        if (hasPistol == true)
        {
            pistolIcon.SetActive(true);
            knifeIcon.SetActive(false);
            AmmoCount.SetActive(true);
        }
        else
        {
            pistolIcon.SetActive(false);
            knifeIcon.SetActive(true);
            AmmoCount.SetActive(false);
        }
    }

    /// <summary>
    /// Call this from your pickup so that the player “now has a pistol.”
    /// </summary>
    public void GrantPistol()
    {
        hasPistol = true;
        // (Optionally, you could disable the text update here and only write UI once, 
        //  but Update() is fine for a simple status label.)
    }
}