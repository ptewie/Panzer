using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMinePickup : MonoBehaviour
{
    public LandMinePowerup landMinePowerup;

    private void OnTriggerEnter(Collider other)
    {
        PowerupManager manager = other.transform.parent.GetComponent<PowerupManager>();
        if (manager != null)
        {
            manager.Add(landMinePowerup);
            Destroy(gameObject);
        }
    }
} 
