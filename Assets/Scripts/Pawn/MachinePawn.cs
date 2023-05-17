using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePawn : Pawn
{
    private const float forwardDirection = 1f;
    private const float backwardDirection = -1f;
    public float forwardMoveSpeed = 10f;
    public float backwardMoveSpeed = 9f; 
    
    public override void MoveBackward()
    {
        mover.Move(backwardMoveSpeed, backwardDirection);
        base.MoveBackward();
    }

    public override void MoveForward()
    {
        mover.Move(forwardMoveSpeed, forwardDirection); 
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
        mover = GetComponent<MachineMover>();
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}
