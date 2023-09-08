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
    public bool isShooting;
    public KeyCode pauseKeyCode;
    //Game Objects and classes

    // Start is called before the first frame update
    public override void Start() //overides Controller Start
    {
        pawn = GetComponent<MachinePawn>(); // assigns the player's pawn as MachinePawn
        
        if (GameManager.Instance) //Does the game manger exist
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
            pawn.MoveForward();
        }
        if (Input.GetKey(backwardKeyCode))
        {
            pawn.MoveBackward();
        }
        if (Input.GetKey(leftKeyCode))
        {
            pawn.Rotate(-1f); //f converts it from a float to an int, saves a step

        }
        if (Input.GetKey(rightKeyCode))
        {
            pawn.Rotate(1f);
        }

        if (Input.GetKeyDown(shootKeyCode))
        {
            pawn.Shoot();
        }

        if (Input.GetKeyDown(pauseKeyCode))
        {
            GameManager.Instance.TogglePause();
          }
    }
}
