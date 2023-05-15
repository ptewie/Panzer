using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePawn : Pawn
{
    public override void MoveBackward()
    {
        Debug.Log("Move backwards");
        base.MoveBackward();
    }

    public override void MoveForward()
    {
        Debug.Log("Move forwards");
        base.MoveForward();
    }

    public override void Rotate(float direction)
    {
        Debug.Log("RO- TAT -ey");
        base.Rotate(direction);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}
