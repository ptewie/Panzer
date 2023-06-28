using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MachineMover))]
[RequireComponent(typeof(MachineShooter))]
public class MachinePawn : Pawn
{
    private const float forwardDirection = 1f;
    private const float backwardDirection = -1f;
    public float forwardMoveSpeed = 10f;
    public float backwardMoveSpeed = 9f;
    public float machineRotationSpeed = 10f;
    public float fireForce = 1000f;
    public float damageDone = 10f;
    public float shellLifespan = 15f;
    public GameObject shellPrefab;
    public float shotCooldownTimer = 1f;
    private float secondsSinceLastShot = Mathf.Infinity;



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
        mover.Rotate(machineRotationSpeed, direction);
        base.Rotate(direction);
    }

    public override void RotateTowards(Vector3 targetPosition)
    {
        Vector3 vectorToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, machineRotationSpeed * Time.deltaTime);
    }


// Start is called before the first frame update
public override void Start()
    {
        shooter = GetComponent<MachineShooter>();
        mover = GetComponent<MachineMover>();
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        secondsSinceLastShot += Time.deltaTime;
        base.Update();
    }

    public override void Shoot()
    {
        if (secondsSinceLastShot > shotCooldownTimer)
        {
            shooter.Shoot(shellPrefab, fireForce, damageDone, shellLifespan);
            secondsSinceLastShot = 0f;
            base.Shoot();
        }
    }
}
