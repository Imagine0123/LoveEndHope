using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public int quantity;

    private void OnValidate()
    {
        if (itemSO == null)
        {
            return;
        }

        spriteRenderer.sprite = itemSO.icon;
        this.name = itemSO.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.Play("LootPickup");
            Destroy(gameObject, .5f);
        }
    }
}
