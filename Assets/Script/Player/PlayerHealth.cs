using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    public Slider slider;
    public GameObject deathScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        slider.value = health;
        SoundManager.instance.PlaySound2D("PlayerDamage");
        if (health <= 0 || transform.position.y <= -20f)
        {
            deathScreen.SetActive(true);
            SoundManager.instance.PlaySound2D("PlayerDeath");
            Destroy(gameObject);
        }
    }
}

