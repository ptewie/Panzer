using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameStateChangedEvent : UnityEvent<GameState, GameState>
{

}

public enum GameState { TitleState, OptionsState, GameplayState, GameOverState, Credits, Pause, Victory};



[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    // Setting up game manager that can be referenced from any class in the project. 
    public static GameManager Instance;
    public List<int> points = new List <int>();
    public List<int> lives = new List<int>();
    public List<Controller> players = new List<Controller>();
    public List<Controller> enemies = new List<Controller>();
    public List<PawnSpawnPoint> pawnSpawnPoints = new List<PawnSpawnPoint>();
    public List<Waypoint> waypoints = new List<Waypoint>();
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public int maxEnemyCount = 4;
    public int numberOfPlayers = 1; //by default max amount of player is 1
    public GameStateChangedEvent OnGameStateChanged = new GameStateChangedEvent();
    public GameState currentGameState;
    private GameState previousGameState;
    public PlayerHUDManager currentPlayerHUD;
    public int targetPointTotal = 600; // point total can be set by designers in editor
    public Pawn playerPawnRef;

    // Method to check for the win condition
    public bool CheckWinCondition(int currentPointTotal)
    {
        return currentPointTotal >= targetPointTotal;
    }


public IEnumerator SpawnMachineNextFrame()
    {
        yield return null;
        Debug.Log("babaeaba");
        // Write code here
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

    }

    void Update()
    {
            int currentPointTotal = CalculateCurrentPointTotal();
            if (CheckWinCondition(currentPointTotal))
            {
            // Win condition reached!
            CallVictory();
            }
        
    }

    public int CalculateCurrentPointTotal()
    {
        int currentPointTotal = 0;

        // Loop through all players and add their points to the total
        foreach (Controller player in players)
        {
            currentPointTotal += player.points / 2;
        }

        // Debug log to show the current point total
        Debug.Log("Current point total: " + currentPointTotal);

        return currentPointTotal;
    }

    public void ChangePlayerAmount(bool isMultiplayer)
    {
        if (isMultiplayer) 
        {
            numberOfPlayers = 2;
        }
        else 
        {
            numberOfPlayers = 1;
        }

    }
    public bool PlayersHaveLives
    {
        get
        {
            int totalLives = 0;
            foreach (PlayerController player in players)
            {
                totalLives += player.lives;
            }
            return (totalLives > 0);
        }
    }

    public int GetPlayerIndex(Pawn source)
    {
        foreach (Controller controller in players)
        {
            if (controller.ControlledPawn == source)
            {
                return (players.IndexOf(controller));
            }
        }

        return -1;
    }

    public void ChangeGameState(GameState state)
    {
        previousGameState = currentGameState;
        currentGameState = state;
        OnGameStateChanged.Invoke(previousGameState, currentGameState);
    }
    public void SpawnPlayer()
    {
        while (pawnSpawnPoints.Count <= numberOfPlayers)
        {
            Debug.LogError("Not enough spawn points");
            return;
        }
        PawnSpawnPoint spawn = GetRandomSpawnPoint();
        if (spawn.spawnedPawn == null)
        {
            GameObject spawnedPlayer = Instantiate(playerPrefab, spawn.transform.position, Quaternion.identity);
            spawn.spawnedPawn = spawnedPlayer.GetComponent<Pawn>();
            currentPlayerHUD = spawnedPlayer.GetComponent<PlayerHUDManager>(); //setting hud
            players.Add(spawnedPlayer.GetComponent<Controller>());
            // make sure enough spawn points exist so that the game dosen't end up breaking. 
            //AdjustPlayerCameras();
        }
        else
        {
            
        }
    }

    public void ResetPlayer(Controller spawnedPlayer) 
    {
        PawnSpawnPoint spawn = GetRandomSpawnPoint();
        spawnedPlayer.GetComponent<Health>().ResetHealth();
        spawnedPlayer.gameObject.transform.SetPositionAndRotation(spawn.transform.position, Quaternion.identity);
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
        if (players.Count < numberOfPlayers)
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
        if (players.Count == 0)
        {
            Debug.Log("only one player");
            // get player 1 came
            Camera player1Camera = players[0].GetComponentInChildren<Camera>();

            // Set player 1's camera location
            Debug.Log(player1Camera.rect);
            player1Camera.rect = new Rect(0, 0, 1f, 1f);

        }
        else
        {
            Debug.Log("two players, setting split screen mode ");
            // get player 1 and player twos camera
            Camera player1Camera = players[0].GetComponentInChildren<Camera>();

            // set player 1 camera location
            player1Camera.rect = new Rect(0, 0, 0.5f, 1f);

            // set player 2's camera location
           // player2Camera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        }
    }
    public Waypoint GetRandomWaypoint()
    {
        if (waypoints.Count > 0) 
        { return waypoints[UnityEngine.Random.Range(0, waypoints.Count)]; }
        else 
        {
            return null;
        }

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
        Debug.Log("please for the love of god work");

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

    public void CallGameOver() 
    {
        ChangeGameState(GameState.GameOverState);
        Time.timeScale = 1f;
    }

    public void CallVictory() 
    {
        ChangeGameState(GameState.Victory);
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
