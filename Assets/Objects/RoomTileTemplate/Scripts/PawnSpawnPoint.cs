using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawnPoint : MonoBehaviour
{
    public Pawn spawnedPawn;
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.pawnSpawnPoints.Add(this);
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        GameManager.Instance.pawnSpawnPoints.Remove(this);
    }
}
