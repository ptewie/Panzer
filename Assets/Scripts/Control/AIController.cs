using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class AIController : Controller  // asbtract means it cannot be instaitated
{
    public enum AIState {  Idle, Chase, Flee, Patrol, Attack, Scan, BackToPost };

    public float attackRange = 100f;
    public AIState currentState = AIState.Scan; //Default State
    private float lastStateChangeTime = 0f;
    public GameObject target;
    public Transform post;
    public float fieldOfView = 30f;


    public override void Start()
    {
        pawn = GetComponent<Pawn>();
        post = transform; //Whereever the enemy is spawned is the idle spot
        base.Start();
    }

    public override void Update()
    {
        MakeDecisions();
        base.Update();  
    }

    public void MakeDecisions() 
    {
        switch (currentState)
        {
            case AIState.Idle:
                //Do the states behavior
                DoIdleState();
                //check for transitions
                foreach (Controller playerController in GameManager.Instance.players) 
                {
                    
                    if (CanSee(playerController.gameObject)) 
                    {
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                        return;//one decision is made, stop running the code
                    }
                    if (CanHear(playerController.gameObject)) 
                    {
                        ChangeAIState(AIState.Scan);
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
                if (!CanSee(target))
                {
                    target = null;
                    ChangeAIState(AIState.Scan);
                    return;
                }
                break;

            case AIState.Chase:
                //Do the states behavior
                DoChaseState();
                //check for transitions
                if (!CanSee(target)) 
                {
                    target = null;
                    ChangeAIState(AIState.Scan);
                    return;
                }
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
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                        return; //one decision is made, stop running the code
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
                if (Vector3.SqrMagnitude(target.transform.position - transform.position) <= 1f)
                {
                    ChangeAIState(AIState.Idle);
                    return;
                }
                break;

            default:
                Debug.LogWarning("AI controller does not have state implemented");
                break;
            
        }
    }

    private bool CanHear(GameObject targetGameObject)
    {
        // looking for target noise maker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();  
    }

    private bool CanSee(GameObject targetGameObject)
    {
        Vector3 agentToTargetVector = targetGameObject.transform.position - transform.position;
        if (Vector3.Angle(agentToTargetVector, transform.forward) <= fieldOfView) 
        {
            Debug.Log("I see a player!");
            Vector3 raycastDirection = targetGameObject.transform.position - pawn.transform.position;
            RaycastHit hit;
            Physics.Raycast(transform.position, raycastDirection, out hit);
            //NOTE: Yes, This code is way different than what was offered in the example, but otherwise I get an error saying that the target
            // game object is not set to an instance. Magic code ig?
            if (Physics.Raycast(transform.position, raycastDirection, out hit)) 
            {
                if (hit.collider.transform.parent != null) 
                {
                    return (hit.collider.transform.parent.gameObject == targetGameObject);
                }
                
            
            }
            
        }
        return true;
    }

    private void DoIdleState() 
    {
        // throw new NotImplementedException(); 

    }

    private void DoAttackState()
    {
        // throw new NotImplementedException();
        pawn.RotateTowards(target.transform.position);
        pawn.Shoot();
    }

    private void DoChaseState()
    {
        // throw new NotImplementedException();
        //Turn to face target
        pawn.RotateTowards(target.transform.position);
        // Move forward if facing target
        pawn.MoveForward();

    }

    private void DoPatrolState() 
    {
        // throw new NotImplementedException();
    }
    private void DoScanState()
    {
        //Rotate Clockwise
        pawn.Rotate(1f);
    }
    private void DoFleeState()
    {
        // throw new NotImplementedException();
    }
    private void DoBackToPostState()
    {
        // throw new NotImplementedException();
        pawn.RotateTowards(post.transform.position);
        pawn.MoveForward();
    }

    public void ChangeAIState(AIState newState) 
    {
        lastStateChangeTime = Time.time;
        currentState = newState;

    }
}

