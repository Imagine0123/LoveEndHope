using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void takeDamage(int damage)
    {
        SoundManager.instance.PlaySound2D("ZombieDamage");
        health -= damage;
        if (health <= 0)
        {
            SoundManager.instance.PlaySound2D("ZombieDeath");
            Destroy(gameObject);
        }
    }
}
