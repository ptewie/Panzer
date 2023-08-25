using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Setting up game manager that can be referenced from any class in the project. 
    public static GameManager Instance;
    //example points variable
    public int points = 0;
    public List<Controller> players = new List<Controller>();
    public List<Controller> enemies = new List<Controller>();
    public List<PawnSpawnPoint> pawnSpawnPoints = new List<PawnSpawnPoint>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; //If there is no game instance on start up...then set the game instance variable. 
        }
        else
        {
            Debug.LogWarning("trying to create a second instance of the game manager.");
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject); //this.Gameobject gets the owner
    }
    void Start() 
    {
        
    }

}
