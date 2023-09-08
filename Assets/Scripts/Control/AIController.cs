using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AIController : Controller  // asbtract means it cannot be instaitated
{
    public enum AIState { Idle, Chase, Flee, Patrol, Attack, Scan, BackToPost };

    public float attackRange = 100f;
    public AIState currentState = AIState.Idle; //Default State
    public float lastStateChangeTime = 0f;
    public GameObject target;
    public Vector3 targetNoise;
    public Transform post;
    public float fieldOfView = 30f;
    public float hearingDistance = 50f;
    public float fleeDistance;
    public Transform[] patrolpoints;
    public float patrolStopDistance;
    private int currentPatrolpoint = 0;
    public Waypoint currentWaypoint;



    public override void Start()
    {
        if (GameManager.Instance) //Does the game manger exist
        {
            GameManager.Instance.enemies.Add(this); //if so, add PlayerController to the List, which then transfers to "players" list
        }
        pawn = GetComponent<Pawn>();
        post = transform; //Whereever the enemy is spawned is the idle spot
        currentWaypoint = GameManager.Instance.GetRandomWaypoint();
        base.Start();
    }

    public override void Update()
    {
        MakeDecisions();
        base.Update();  
    }

    public virtual void MakeDecisions() 
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
                        Debug.Log("I found ya!");
                        target = playerController.gameObject;
                        ChangeAIState(AIState.Chase);
                        return; //one decision is made, stop running the code
                    }
                    if (CanHear(playerController.gameObject)) 
                    {
                        Debug.Log("I can hear you!!");
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

    protected bool CanHear(GameObject targetGameObject)
    {
        if (Vector3.Distance(transform.position, targetGameObject.transform.position) <= hearingDistance) 
        {
            //.. after this we actually DO hear the player
            targetNoise = targetGameObject.transform.position;
            return true;
        }
        else 
        {
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
                    Debug.Log(hit.collider.gameObject.name);
                    Debug.DrawRay(transform.position, raycastDirection, Color.yellow);
                    return (hit.collider.gameObject == targetGameObject);
                }
            }
            return true;
        }
        else
        {
            return false; //Can't see player, but likley can hear. 
        }
        
    }

    //protected void 


    public void RestartPatrolState()
    {
        //set the currentPatrolpoint index to 0
        currentPatrolpoint = 0;
    }

    public void DoIdleState() 
    {
        // throw new NotImplementedException(); 

    }

    public void DoAttackState()
    {
        pawn.RotateTowards(target.transform.position);
        pawn.MoveForward();
        pawn.Shoot();
    }

    public virtual void DoChaseState()
    {
        //Turn to face target
        pawn.RotateTowards(target.transform.position);
        // Move forward if facing target
        pawn.MoveForward();

    }

    public void DoPatrolState() 
    {
        // if we have enough waypoints in the list...
        if (patrolpoints.Length > currentPatrolpoint)
        {
            // go to the current waypoint
            pawn.RotateTowards(patrolpoints[currentPatrolpoint].transform.position);
            pawn.MoveForward();
            // if we're close enough to the next waypoint, increment to the next waypoint
            if (Vector3.Distance(pawn.transform.position, patrolpoints[currentPatrolpoint].position) < patrolStopDistance) 
                {
                currentPatrolpoint++;
                }
        } else //when last item of array is met, else is called and the function essentially loops 
        {
            RestartPatrolState();
        }
    }


    public virtual void DoScanState()
    {
        //Rotate towards target
        pawn.RotateTowards(targetNoise);
        return;
    }
    public void DoFleeState()
    {
        //Back away in the opposite direction
        pawn.MoveBackward();


    }
    public void DoBackToPostState()
    {

        pawn.RotateTowards(post.transform.position);
        pawn.MoveForward();
        return;
    }

    public void ChangeAIState(AIState newState) 
    {
        lastStateChangeTime = Time.time;
        currentState = newState;

    }
}

