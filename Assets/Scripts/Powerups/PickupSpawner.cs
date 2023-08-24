using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab; //The specific prefab set
    public float spawnDelay;

    private GameObject spawnedPickup;  //has a pick up been spawned yet?
    private float nextSpawnTime;
    private Transform tf;

    void Start()
    {
        spawnedPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity) as GameObject;
    }

    void Update()
    {
        //if there's nothing spawned...
        if (spawnedPickup != null) 
        { 
            // ...now it's time to spawn!
            if (Time.time > nextSpawnTime) 
            {
                //spawn the pickup and set the time for the next one 
                spawnedPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity) as GameObject;
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else 
        {
            //alternativly, if the object is still there, just postpone the nextSpawnTime!
            nextSpawnTime = Time.time + spawnDelay;
        }
        
    }
}
