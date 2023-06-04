using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Setting up game manager that can be referenced from any class in the project. 
    public static GameManager Instance;
    //example points variable
    public int points = 0;
    public List<PlayerController> players = new List<PlayerController>();
    public List<AIController> enemies = new List<AIController>();

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
