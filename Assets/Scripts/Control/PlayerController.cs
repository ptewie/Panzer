using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MachinePawn))]
public class PlayerController : Controller
{
    //Key Codes
    public KeyCode forwardKeyCode;
    public KeyCode backwardKeyCode;
    public KeyCode leftKeyCode;
    public KeyCode rightKeyCode;
    public KeyCode shootKeyCode;
    //Game Objects and classes
    private MachinePawn playerPawn;
    // Start is called before the first frame update
    public override void Start() //overides Controller Start
    {
        playerPawn = GetComponent<MachinePawn>(); // assigns the player's pawn as MachinePawn
        
        if (GameManager.Instance) //Does the gamemanger exist
        {
            GameManager.Instance.players.Add(this); //if so, add PlayerController to the List, which then transfers to "players" list
        }
        
        
        base.Start();                         
    }

    private void OnDestroy() //When you stop simulating, everything technically gets destroyed, don't put crazy stuff in OnDestroy. 
    {
        if (GameManager.Instance) 
        {
            GameManager.Instance.players.Remove(this); //removes PlayerController from the List. 
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        ProcessInputs();
        base.Update();
    }

    private void ProcessInputs()
    {
        if (Input.GetKey(forwardKeyCode))
        {
            playerPawn.MoveForward();
        }
        if (Input.GetKey(backwardKeyCode))
        {
            playerPawn.MoveBackward();
        }
        if (Input.GetKey(leftKeyCode))
        {
            playerPawn.Rotate(-1f); //f converts it from a float to an int, saves a step

        }
        if (Input.GetKey(rightKeyCode))
        {
            playerPawn.Rotate(1f);
        }

        if (Input.GetKeyDown(shootKeyCode))
        {
            playerPawn.Shoot();
        }
    }
}
