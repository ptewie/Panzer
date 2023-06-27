using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    public Transform firepointTransform;
    //public GameObject projectile;
    //public Vector3 firePoint = new Vector3(0f, 0.6f, 1f); //TO DO: change to reflect better for washing machine
    public abstract void Start();
  
    public abstract void Update();

    public abstract void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifespan);


}
