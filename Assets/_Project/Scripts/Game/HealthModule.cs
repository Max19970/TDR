using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthModule : MonoBehaviour
{
    [Header("Health parameters")]
    public float currentHealth = 100;
    public float maxHealth = 100;
    public bool infiniteHealth;

    [Header("Health events")]
    public UnityEvent<float, float> onHealthChange;
    public UnityEvent<float, float> onMaxHealthChange;

    [Header("Debug settings")]
    [SerializeField] private bool enableDebug = true;

    private void Awake()
    {
        if (enableDebug)
        {
            onHealthChange.AddListener((float delta, float currentHealth) => { Debug.Log($"Changed health on {delta}. Current health: {currentHealth}", this); });
            onMaxHealthChange.AddListener((float delta, float maxHealth) => { Debug.Log($"Changed max health on {delta}. Current max health: {currentHealth}", this); });
        }
    }

    public float ChangeHealth(float delta) 
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
        if (infiniteHealth) currentHealth = maxHealth;
        onHealthChange.Invoke(delta, currentHealth);
        return currentHealth;
    }

    public float ChangeMaxHealth(float delta)
    {
        maxHealth += delta;
        onMaxHealthChange.Invoke(delta, maxHealth);
        return maxHealth;
    }

    public void SetCurrentHealth(float value) 
    {
        float oldHealth = currentHealth;
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        onHealthChange.Invoke(currentHealth - oldHealth, currentHealth);
    }

    public void SetMaxHealth(float value)
    {
        float oldMaxHealth = maxHealth;
        maxHealth = value;
        onHealthChange.Invoke(maxHealth - oldMaxHealth, maxHealth);
    }

    public float GetCurrentHealth() 
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
