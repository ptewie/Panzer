using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMinePowerup : Powerup
{
    public float damageToDeal;
    public Pawn owner;


    public override void Apply(PowerupManager target)
    {
        Health targetHealth = target.gameObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            Debug.Log("Ouch!");
           targetHealth.TakeDamage(damageToDeal, owner);
        }
        else
        {
            Debug.Log("this should never happen!");
        }
    }

    public override void Remove(PowerupManager target)
    {
        // dont 
    }
}