using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerDelta : AIController
{
    //Defining RNG
    public float coinFlip = 0;
    public override void Start()
    {

        coinFlip = Random.Range(0f, 100f);
        base.Start();
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
                        ChangeAIState(AIState.Scan);
                        return;
                    }
                    else
                    {
                        Debug.Log("Nothing going on here!");
                        ChangeAIState(AIState.Idle);
                        return;
                    }
                }
            break;

            case AIState.Scan:
                //Do the states behavior
                DoScanState();
                //check for transitions
                foreach (Controller playerController in GameManager.Instance.players)
                {
                    if (CanSee(playerController.gameObject))
                    target = playerController.gameObject;
                    {
                        if (coinFlip >= 50f) 
                        {
                            ChangeAIState(AIState.Flee);
                        }
                        if (coinFlip <= 50f) 
                        {
                            ChangeAIState(AIState.Chase);
                        }
                        
                        Debug.Log("Found you!");
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                        return;
                    }
                }
                if (Time.time - lastStateChangeTime > 3f)
                {
                    ChangeAIState(AIState.Idle);
                    return;
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
           
            case AIState.Flee:
                //Do the states behavior  
                DoFleeState();
                if (Time.time - lastStateChangeTime > 3f)
                {
                    ChangeAIState(AIState.Scan); // looping back to scan
                    return;
                }
                //check for transitions
                if (Vector3.SqrMagnitude(target.transform.position - transform.position) <= attackRange)
                {
                    ChangeAIState(AIState.Attack);
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
    