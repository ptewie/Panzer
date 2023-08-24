using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerBravo : AIController
{
    public override void DoChaseState()
    {
        //Turn to face target
        pawn.RotateTowards(target.transform.position);
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
                ChangeAIState(AIState.Patrol); //Default state should be patrol, this is fail safe. 
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

            case AIState.Chase: //When chasing, enemy totally abandons post and rushes in on the player.
                //Do the states behavior
                DoChaseState();
                if (Vector3.SqrMagnitude(target.transform.position - transform.position) <= attackRange)
                {
                    ChangeAIState(AIState.Attack);
                    return;
                }
                break;

            case AIState.Patrol:
                //Do the states behavior
                DoPatrolState();
                //check for transitions
                foreach (Controller playerController in GameManager.Instance.players)
                {
                    if (CanSee(playerController.gameObject))
                    {
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                    }
                    //function should constantly loop until player is heard!!!
                }
                    break;

            default:
                Debug.LogWarning("AI controller does not have state implemented");
                break;

        }
    }

}

