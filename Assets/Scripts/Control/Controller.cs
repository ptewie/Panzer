using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour // asbtract means it cannot be instaitated
{
    protected Pawn pawn;
    public Pawn ControlledPawn 
    { 
        get 
        { 
            return pawn; 
        }

    }
    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }
}

