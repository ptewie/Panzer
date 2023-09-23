using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour // asbtract means it cannot be instaitated
{
    protected Pawn pawn;
    public int points;
    public int lives;

    public Pawn ControlledPawn 
    { 
        get 
        { 
            return pawn; 
        }
        
    }

    public abstract void removeLife();

    public abstract void addPoints(int pointsToAdd);
    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }
}

