using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawner : PickupSpawner
{
    // Start is called before the first frame update

    public PawnSpawnPoint spawnPoint;
    void Start()
    {
        GetComponent<PawnSpawnPoint>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
