using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damage = 20f;
    public Pawn owner; //leaving null for now

    public void OnTriggerEnter(Collider other)
    {
        //get health componenet from game object w/ overlapping collider
        Health otherHealth = other.gameObject.GetComponent<Health>();
        //only deal damage if is has a health component
        if (otherHealth != null)
        {
            // deal that damage
            otherHealth.TakeDamage(damage, owner);

        }
        // destory object whether or not damaage has actually been dealt
        Destroy(gameObject); 
    }

}
