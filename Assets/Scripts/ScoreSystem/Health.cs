using UnityEngine;
using UnityEngine.Events;

// place this script on a GameObject for the player's health
// and connect the OnHealthChanged to UI health bar
// and OnDeath to whatever must happen when defeating the oponent :)
public class Health : MonoBehaviour{

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public UnityEvent OnHealthChanged; // Connect this with HealthBar to update the health bar
    public UnityEvent OnDeath; // Connect this with whatever signifies winning

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        
        OnHealthChanged?.Invoke();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // No overhealing. Feel free to change this if you want
        
        OnHealthChanged?.Invoke();
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public int GetCurrentHealth() => currentHealth;
}