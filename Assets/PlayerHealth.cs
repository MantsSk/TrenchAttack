using UnityEngine;
using UnityEngine.UI; // If you want to include UI elements like health bars
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the player
    public int currentHealth; // Current health of the player
    public Slider healthBar; // Health bar UI element

    public float invincibilityTime = 2f; // Time during which the player is invincible after taking damage
    private bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return; // Ignore damage if invincible
        }

        currentHealth -= damage; // Reduce health by damage amount
        if (currentHealth <= 0)
        {
            Die(); // Call Die method if health is zero or below
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine()); // Start invincibility period
        }

        UpdateHealthBar(); // Update the health bar UI
    }

    // Method to heal the player
    public void Heal(int amount)
    {
        currentHealth += amount; // Increase health by healing amount
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Ensure health does not exceed max health
        }

        UpdateHealthBar(); // Update the health bar UI
    }

    // Method to handle player death
    private void Die()
    {
        // Add logic for player death, e.g., play animation, restart level, etc.
        Debug.Log("Player has died!");
    }

    // Coroutine to handle invincibility period after taking damage
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime); // Wait for invincibility time
        isInvincible = false;
    }

    // Method to update the health bar UI
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }
}
