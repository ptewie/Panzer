using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameStateChangedEvent : UnityEvent<GameState, GameState>
{

}

public enum GameState { TitleState, OptionsState, GameplayState, GameOverState, Credits, Pause};



[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    // Setting up game manager that can be referenced from any class in the project. 
    public static GameManager Instance;
    //example points variable
    public int points = 0;
    public List<Controller> players = new List<Controller>();
    public List<Controller> enemies = new List<Controller>();
    public List<PawnSpawnPoint> pawnSpawnPoints = new List<PawnSpawnPoint>();
    public List<Waypoint> waypoints = new List<Waypoint>();
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public int maxEnemyCount = 4;
    public int numberOfPlayers = 1;
    public GameStateChangedEvent OnGameStateChanged = new GameStateChangedEvent();
    public GameState currentGameState;
    private GameState previousGameState;





    public IEnumerator SpawnMachineNextFrame()
    {
        // Write code here
        yield return null;
        // This code runs on the next frame
        SpawnPlayers();
        SpawnEnemies();
        //SpawnEnemy();
    }

    public IEnumerator SpawnPlayerMachineNextFrame()
    {
        // Write code here
        yield return null;
        // This code runs on the next frame
        SpawnPlayer();
    }

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
        FindObjectsOfType<PawnSpawnPoint>();
    }

    public void ChangeGameState(GameState state)
    {
        previousGameState = currentGameState;
        currentGameState = state;
        OnGameStateChanged.Invoke(previousGameState, currentGameState);
    }
    public void SpawnPlayer()
    {
        if (pawnSpawnPoints.Count <= numberOfPlayers)
        {
            Debug.LogError("Not enough spawn points");
            return;
        }
        PawnSpawnPoint spawn = GetRandomSpawnPoint();
        if (spawn.spawnedPawn == null)
        {
            GameObject spawnedPlayer = Instantiate(playerPrefab, spawn.transform.position, Quaternion.identity);
            spawn.spawnedPawn = spawnedPlayer.GetComponent<Pawn>();
            players.Add(spawnedPlayer.GetComponent<Controller>());
            // make sure enough spawn points exist so that the gane dosen't end up breaking. 
            AdjustPlayerCameras();
        }
        else
        {
            SpawnPlayer();
        }
    }

    public void SpawnEnemies()
    {
        while (enemies.Count < maxEnemyCount)
        {
            Debug.Log("Enemy Count: " + enemies.Count);
            Debug.Log("Max Number of Enemies: " + maxEnemyCount);
            SpawnEnemy();
        }
    }
    public void SpawnPlayers()
    {
        
        while (players.Count < numberOfPlayers)
        {
            Debug.Log("Player Count: " + players.Count);
            Debug.Log("Number of Players: " + numberOfPlayers);
            SpawnPlayer();
            
        }
    }
    public void SpawnEnemy()
    {
        if (pawnSpawnPoints.Count <= (players.Count + enemies.Count))
        {
            Debug.LogError("Need spawn points");
            return;
        }
        PawnSpawnPoint spawn = GetRandomSpawnPoint();
        if (spawn.spawnedPawn == null)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawn.transform.position, Quaternion.identity);
            spawn.spawnedPawn = spawnedEnemy.GetComponent<Pawn>();
            enemies.Add(spawnedEnemy.GetComponent<Controller>());
            // MAKE SURE THERE ARE ENOUGH PAWN SPAWN POINTS SO THE GAME NEVER BREAKS
        }
        else
        {
            SpawnEnemy();
        }

    }

    private PawnSpawnPoint GetRandomSpawnPoint()
    {
       return pawnSpawnPoints[UnityEngine.Random.Range(0, pawnSpawnPoints.Count)];
    }

    public void AdjustPlayerCameras()
    {
        if (players.Count == 1)
        {
            // get player 1 came
            Camera player1Camera = players[0].GetComponentInChildren<Camera>();

            // Set player 1's camera location
            Debug.Log(player1Camera.rect);
            player1Camera.rect = new Rect(0, 0, 1f, 1f);

        }
        else
        {
            // get player 1 and player twos camera
            Camera player1Camera = players[0].GetComponentInChildren<Camera>();
            Camera player2Camera = players[1].GetComponentInChildren<Camera>();

            // set player 1 camera location
            player1Camera.rect = new Rect(0, 0, 0.5f, 1f);

            // set player 2's camera location
            player2Camera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        }
    }
    public Waypoint GetRandomWaypoint()
    {
        return waypoints[UnityEngine.Random.Range(0, waypoints.Count)];

    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting");
    }

    public void StartGame()
    {
        ChangeGameState(GameState.GameplayState);
        Time.timeScale = 1f;
        StartCoroutine(SpawnMachineNextFrame());
    }

    public void OpenOptionsMenu()
    {
        ChangeGameState(GameState.OptionsState);
    }

    public void CloseOptionsMenu()
    {
        ChangeGameState(previousGameState);
    }

    public void ChangeToPreviousGameState()
    {
        ChangeGameState(previousGameState);
    }

    public void ChangeStateToTitle()
    {
        ChangeGameState(GameState.TitleState);
    }

    public void PauseGame()
    {
        ChangeGameState(GameState.Pause);
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        ChangeGameState(GameState.GameplayState);
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        if (currentGameState == GameState.Pause)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }
}
