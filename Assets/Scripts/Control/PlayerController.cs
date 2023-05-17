using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MachinePawn))]
public class PlayerController : Controller
{
    public KeyCode forwardKeyCode;
    public KeyCode backwardKeyCode;
    public KeyCode leftKeyCode;
    public KeyCode rightKeyCode;
    private MachinePawn playerPawn;
    // Start is called before the first frame update
    public override void Start() //overides Controller Start
    {
        playerPawn = GetComponent<MachinePawn>(); // <- tells the function what it returns in the angle brackets ("<>") , so it looks at every 
        base.Start();                             // component on the same game object it's on. 
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
    }
}
