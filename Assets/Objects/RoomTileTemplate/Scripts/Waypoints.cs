using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Waypoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.waypoints.Add(this);
    }

    private void OnDestroy()
    {
        GameManager.Instance.waypoints.Remove(this);
    }
}