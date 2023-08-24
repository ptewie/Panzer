using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damage = 20f;
    public Pawn owner; //leaving null for now

    private void OnCollisionEnter(Collision other)
    {
        // Get the Health component from the Game Object that has the Collider that we are overlapping
        Health otherHealth = other.gameObject.GetComponent<Health>();
        // Only damage if it has a Health component
        if (otherHealth)
        {
            // Do damage
            otherHealth.TakeDamage(damage, owner);
        }
        else
        {
            //Debug.Log(other.gameObject.name);
            return;
        }

        // Destroy ourselves, whether we did damage or not
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Get the Health component from the Game Object that has the Collider that we are overlapping
        Health otherHealth = other.GetComponent<Health>();
        // Only damage if it has a Health component
        if (otherHealth)
        {
            // Do damage
            otherHealth.TakeDamage(damage, owner);
        }
        else
        {
            Debug.Log(other.name);
            return;
        }

        // Destroy ourselves, whether we did damage or not
        Destroy(gameObject);
    }
}