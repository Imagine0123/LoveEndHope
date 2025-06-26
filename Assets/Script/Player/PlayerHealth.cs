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
        if (damage > 0)
        {
            SoundManager.instance.PlaySound2D("PlayerDamage");
        }
        else if (damage < 0)
        {
            SoundManager.instance.PlaySound2D("PlayerHeal");
        }
        if (health <= 0)
        {
            deathScreen.SetActive(true);
            SoundManager.instance.PlaySound2D("PlayerDeath");
            Destroy(gameObject);
        }
    }
}

