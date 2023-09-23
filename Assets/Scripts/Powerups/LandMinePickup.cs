using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMinePickup : MonoBehaviour
{
    public LandMinePowerup landMinePowerup;

    private void OnTriggerEnter(Collider other)
    {
        PowerupManager manager = other.GetComponent<PowerupManager>();
        if (manager)
        {
            manager.Add(landMinePowerup);
            Destroy(gameObject);
        }
    }
} 
