using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIController : Controller  // asbtract means it cannot be instaitated
{
    public enum AIState {  Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost };

    public AIState currentState = AIState.Chase; //Defualt State
    private float lastStateChangeTime = 0f;
    public GameObject target;


    public override void Start()
    {
        pawn = GetComponent<Pawn>();
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
            case AIState.Guard:
                //Do the states behavior
                DoGuardState();
                //check for transitions
                break;
            case AIState.Attack:
                //Do the states behavior
                DoAttackState();
                //check for transitions
                break;
            case AIState.Chase:
                //Do the states behavior
                DoChaseState();
                //check for transitions
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
                break;
            case AIState.BackToPost:
                //Do the states behavior
                DoBackToPostState();
                //check for transitions
                break;
            default:
                Debug.LogWarning("AI controller does not have state implemented");
                break;
            
        }
    }

    private void DoGuardState() 
    {
       // throw new NotImplementedException();

    }

    private void DoAttackState()
    {
       // throw new NotImplementedException();
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
        // throw new NotImplementedException();
    }
    private void DoFleeState()
    {
        // throw new NotImplementedException();
    }
    private void DoBackToPostState()
    {
        // throw new NotImplementedException();
    }

    public void ChangeAIState(AIState newState) 
    {
        lastStateChangeTime = Time.time;
        currentState = newState;

    }
}

