using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour 
{
    public int maxHealth;
    public int currentHealth;
    public Slider uIBar;
    public UnityEvent onDie;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUIBar();
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateUIBar();
    }

    public void TakeDamage(int amount)
    {
        UpdateHealth(-amount);
        if (currentHealth <= 0)
        {
            Die();
        }
        if (GetComponent<HealthColorModifier>() != null)
        {
            GetComponent<HealthColorModifier>().ChangeColorTransparency(GetPercentageHealth());
        }
    }

    public void AddHealth(int amount)
    {
        UpdateHealth(amount);
    }

    public void RecoverAllHealth()
    {
        UpdateHealth(maxHealth);
    }

    public float GetPercentageHealth()
    {
        return (float)currentHealth / maxHealth;
    }

    public void Die()
    {
        onDie.Invoke();
    }

    public void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }

    public void UpdateUIBar()
    {
        if (uIBar != null)
        {
            uIBar.value = GetPercentageHealth();
        }
    }
}
