using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    protected Mover mover; 
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void MoveForward()
    {

    }

    public virtual void MoveBackward()
    {

    }

    public virtual void Rotate(float direction) //float is telling which DIRECTION to rotate in
    {

    }
}
