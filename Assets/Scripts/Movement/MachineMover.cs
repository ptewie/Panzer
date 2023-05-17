using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineMover : Mover
{
    private Rigidbody machineRigidBody;

    private void Start()
    {
        machineRigidBody = GetComponent<Rigidbody>();

    }
    public override void Move(float moveSpeed, float direction)
    {
        Vector3 currentPosition = transform.position;
        machineRigidBody.MovePosition(currentPosition + (transform.forward * direction * moveSpeed * Time.deltaTime));
        base.Move(moveSpeed, direction);
    }

    public override void Rotate()
    {
        base.Rotate();
    }
}
