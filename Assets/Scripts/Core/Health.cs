using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthChanged : UnityEvent<float, float>
{

}

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public const float minHealth = 0f;
    public float currentHealth;
    public float maxHealth = 100f;
    public HealthChanged OnHealthChanged = new HealthChanged();


    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(float damage, Pawn source)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, minHealth, maxHealth);
        OnHealthChanged.Invoke(currentHealth, maxHealth);
        Debug.Log(source.name + " did " + damage + " damage to " + gameObject.name);
        if (Mathf.Approximately(currentHealth, minHealth))
        {
            Die(source);
        }
    }

    public void ApplyHealing(float value)
    {
        if (value < 0)
        {
            Debug.LogWarning("attempted to heal fort negetive amount (bad)");
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth + value, minHealth, maxHealth);
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    private void Die(Pawn source)
    {
        Destroy(gameObject); //TO DO: Make this not shit. 
    }
}
