using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerCharlie : AIController
{
    public GameObject leader;

    protected void FindNearestPawn()
    {
        //Get a list of all pawns 
        Pawn[] allMachines = FindObjectsOfType<Pawn>();

        //Assume that the first one is the closest one
        Pawn closestMachine = allMachines[0];
        float closestMachineDistance = Vector3.Distance(pawn.transform.position, closestMachine.transform.position);

        //Iterate through list one at a time
        foreach (Pawn machine in allMachines)
        {
            if (Vector3.Distance(pawn.transform.position, machine.transform.position) < closestMachineDistance)
            {
                // this is the closest
                closestMachine = machine;
                closestMachineDistance = Vector3.Distance(pawn.transform.position, closestMachine.transform.position);
            }
        }
        // target the closest machine
        leader = closestMachine.gameObject;

    }

    public override void DoChaseState() //Using the Chase State as a "follow" state to allow Charlie to follow other enemies. 
    {
        //Turn to face target
        pawn.RotateTowards(leader.transform.position);
        // Move forward if facing target
        pawn.MoveForward();

    }
    public override void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:
                //Do the states behavior
                DoIdleState();
                //check for transitions
                foreach (Controller playerController in GameManager.Instance.players)
                {
                    target = playerController.gameObject;
                    ChangeAIState(AIState.Chase);
                }
                break;
            case AIState.Attack:
                //Do the states behavior
                DoAttackState();
                //check for transitions
                if (Vector3.SqrMagnitude(target.transform.position - transform.position) > attackRange)
                {
                    ChangeAIState(AIState.Chase);
                    return;

                }
                break;

            case AIState.Chase:
                //Do the states behavior
                DoChaseState();
                //check for transitions
                // Checking for distance
                if (Vector3.SqrMagnitude(target.transform.position - transform.position) <= attackRange)
                {
                    ChangeAIState(AIState.Attack);
                    return;
                }
                break;

            case AIState.Flee:
                //Do the states behavior  
                DoFleeState();
                //check for transitions
                break;

            case AIState.Patrol:
                //Do the states behavior
                DoPatrolState();
                //check for transitions
                break;

            case AIState.Scan:
                //Do the states behavior
                DoScanState();
                //check for transitions
                foreach (Controller playerController in GameManager.Instance.players)
                {
                    if (CanSee(playerController.gameObject))
                    {
                        Debug.Log("Found you after hearing!");
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                        return; //one decision is made, stop running the code
                    }
                    if (!CanSee(playerController.gameObject))
                    {
                        Debug.Log("I can't find anyone!");
                        ChangeAIState(AIState.Idle);
                        return;
                    }
                }
                if (Time.time - lastStateChangeTime > 3f)
                {
                    ChangeAIState(AIState.BackToPost);
                    return;
                }
                break;

            case AIState.BackToPost:
                //Do the states behavior
                DoBackToPostState();
                //check for transitions
                if (Vector3.SqrMagnitude(post.transform.position - transform.position) <= 1f)
                {
                    Debug.Log("I'm going back home!");
                    ChangeAIState(AIState.Idle);
                    return;
                }
                break;

            default:
                Debug.LogWarning("AI controller does not have state implemented");
                break;

        }
    }
}
