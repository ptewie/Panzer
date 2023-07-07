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
    public float hearingDistance = 0f;



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
                        Debug.Log("I see you!");
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                        return;//one decision is made, stop running the code
                    }
                    if (CanHear(playerController.gameObject)) 
                    {
                        Debug.Log("I can hear you!!");
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
                if (Vector3.SqrMagnitude(post.transform.position - transform.position) <= 1f)
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
        //grab the noise maker of the target!
        var noiseMaker = targetGameObject.GetComponent<NoiseMaker>();
        //if they don't got one, no noise, so it would return false
        if (noiseMaker == null) 
        {
            return false;
        }
        //If they're making exactly 0 noise, they still can't be heard
        if (noiseMaker.volumeDistance <= 0f) 
        { 
            return false;
        }
        //if noise is actually being made add the volume distance to the hearing distance of the AI
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;
        //if the distance is closer than this:
        if (Vector3.Distance(pawn.transform.position, targetGameObject.transform.position) <= totalDistance)
        {
            //then the target is hjeard
            return true;
        }
        else 
        {
            //otherwise, we are too far away to be heard
            return false;
        
        }



    }

    protected bool CanSee(GameObject targetGameObject)
    {
        Vector3 agentToTargetVector = targetGameObject.transform.position - transform.position;
        if (Vector3.Angle(agentToTargetVector, transform.forward) <= fieldOfView)
        {
            Vector3 raycastDirection = targetGameObject.transform.position - pawn.transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, raycastDirection.normalized, out hit))
            {
                if (hit.collider != null)
                {
                    return (hit.collider.gameObject == targetGameObject);
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

