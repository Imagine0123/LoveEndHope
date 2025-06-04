using UnityEngine;

public class Collectable : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) return; // already collected? bail out.

        // Only run this logic if the thing entering is the Player.
        if (collision.gameObject.CompareTag("Player"))
        {
            collected = true;

            // 1) Tell PistolManager: the player now has a pistol
            if (PistolManager.instance != null)
            {
                PistolManager.instance.GrantPistol();
            }
            else
            {
                Debug.LogWarning("[Collectable] No PistolManager.instance found in scene!");
            }

            // 2) Destroy the pickup so it can't be collected again
            Destroy(gameObject);
        }
    }
}
