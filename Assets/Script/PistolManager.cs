using UnityEngine;
using TMPro;

public class PistolManager : MonoBehaviour
{
    public static PistolManager instance;
    public bool hasPistol = false;

    [SerializeField] private TMP_Text pistolText;

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
        if (pistolText != null)
        {
            pistolText.text = hasPistol ? "Pistol" : "No Pistol";
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