using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public HealthPowerup healthPowerup;

    private void OnTriggerEnter(Collider other)
    {
        PowerupManager manager = other.GetComponent<PowerupManager>();
        if (other.transform != null)
        {
            if (manager != null)
            {
                manager.Add(healthPowerup);
                Destroy(gameObject);
            }
        }
    }

}