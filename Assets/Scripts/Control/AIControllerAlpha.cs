using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerAlpha : AIController
{
    // Alpha is meant to be a impulsive attacker. They're quick to chase but also quick to flee. T
    // Higher Speed, lower health / easier to kill. 

    protected void GetNearestProjectile()
    {

    }
    //Overide functions from AIController
    public override void DoScanState()
    {
        pawn.Rotate(1f); //Just keep rotating clockwise, this guy got MAD bamboozled. 
        return;
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

                    if (CanHear(playerController.gameObject)) //FSM is similar to AI Controller, but with some changes to reflect Alpha's "impulsibness"
                    {
                        Debug.Log("I can hear you!!");
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                        return;
                    }
                    else
                    {
                        Debug.Log("Nothing going on here!");
                        ChangeAIState(AIState.Scan);
                        return;
                    }

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
                if (!CanHear(target.gameObject))
                {
                    Debug.Log("Yikes!");
                    ChangeAIState(AIState.Flee);

                }
                break;

            case AIState.Flee:
                //Do the states behavior  
                DoFleeState();
                if (Vector3.SqrMagnitude(target.transform.position - transform.position) <= attackRange)
                {
                    ChangeAIState(AIState.Attack);
                    return;
                }
                if (CanSee(target.gameObject)) 
                {
                    ChangeAIState(AIState.Chase);
                }
                if (Time.time - lastStateChangeTime > 3f)
                {
                    ChangeAIState(AIState.BackToPost);
                    return;
                }
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
                        Debug.Log("Found you!");
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                        return; 
                    }
                    if (!CanSee(playerController.gameObject))
                    {
                        Debug.Log("I can't find anyone!");
                        ChangeAIState(AIState.Idle); //Goes back to idle for "regular attitude" they learned nothing
                        return;
                    }
                }
                if (Time.time - lastStateChangeTime > 3f)
                {
                    ChangeAIState(AIState.BackToPost); //Will continously loop between BackToPost and Scan 
                    return;
                }
                break;

            case AIState.BackToPost:
                //Do the states behavior
                DoBackToPostState();
                //check for transitions
                if (Vector3.SqrMagnitude(post.transform.position - transform.position) <= 1f)
                {
                    Debug.Log("What's going on here?");
                    ChangeAIState(AIState.Scan); //If they get overzealous, they go back on default tactics for a little bit
                    return;
                }
                break;

            default:
                Debug.LogWarning("AI controller does not have state implemented");
                break;

        }
    }
}

