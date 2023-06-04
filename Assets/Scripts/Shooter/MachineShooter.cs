using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MachineShooter : Shooter
{
    public Transform firepointTransform;

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifespan)
    {
        // Setting up projectile ----------------------------------------
        GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation) as GameObject; //spawn in a proj.
        DamageOnHit damageOnHit = newShell.GetComponent<DamageOnHit>(); //adding the values onto the projectile
        if (damageOnHit) //just asking if damageOnHit exists period
        {
            damageOnHit.damage = damageDone;
            damageOnHit.owner = GetComponent<Pawn>(); //NOTE: possibly need reference to pawn? 
        }
        // Moving projectile forward ----------------------------------
        Rigidbody rb = newShell.GetComponent<Rigidbody>();
        if (rb) 
        {
            
            rb.AddForce(firepointTransform.forward * fireForce); //makes the projectile move
        }
        Destroy(newShell, lifespan);
        
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {

    }
}
