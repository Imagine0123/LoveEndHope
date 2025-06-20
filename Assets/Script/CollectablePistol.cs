using UnityEngine;

public class CollectablePistol : MonoBehaviour
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
            if (PickUpManager.instance != null)
            {
                PickUpManager.instance.GrantPistol();
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
