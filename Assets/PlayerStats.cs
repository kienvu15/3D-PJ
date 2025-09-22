using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public TextMeshProUGUI healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpDateHealthText();
    }

    public void UpDateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health.ToString();
        }
        else
        {
            Debug.LogWarning("Health Text is not assigned in the PlayerStats component.");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        UpDateHealthText();

        if (health <= 0)
        {
            Debug.Log("Player is dead.");
        }

    }
    
    public void Heal(int amount)
    {
        health += amount;
        if (health > 100) health = 100;
        UpDateHealthText();
    }
}
