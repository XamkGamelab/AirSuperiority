//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
//    public Rigidbody Rb => GetComponent<Rigidbody>();             //Just for reference how to get component with low resource consumption. Rb is also usable beefore Awake!
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //------------------------------------
    //Note to Toni!
    //Player 1 and 2 could have booleans, i.e. player1ControlsEnabled which makes it easier for
    //pauseState and other states to control players. Boolean could exist in GameManager and be read from playerController
    //or otherwise. What seems to be most suitable way to proceed. And/or playerController script could get isPlaying boolean
    //and act accordingly.

    //NOTE2 to Toni!
    //Player[x] gun information is found from StatsManager.Instance.player[x].CurrentGun. First gun at the beginning of a game [0]. this is an Array.
    //StatsManager.Instance.gun[x]  will hold information of usable guns and their properties. This is an array. Not totally finished yet. There is no method
    //to load every usable gun information to array. Array initialization on game start in GameLoader script.

    //Scenemanager and levels will be done similar way as gun array is done.

    //Note to US!!!
    //We need to design and implement how SceneController should work. Or do we have duplicates? SceneController vs LevelManager? Or do they have their own tasks?
    //------------------------------------

    /* Methods that can be called:
     * GameManager.Instance.StartGame()         Call this when Gameplay needs to be started for the first time
     * GameManager.Instance.BeginNextLevel()    Call this when next level needs to be loaded
     * GameManager.Instance.EndLevel()          Call this when changind level
     * GameManager.Instance.GamePaused()        Call this when GameState isPaused
     * GameManager.Instance.ExitPauseState()    Call this when PauseState != isPaused
     */

    [Header("General controls")]
    public bool isPlaying = false;
    public bool isPaused = false;
    public bool isGameOver = false;                     //Use if needed
    public bool updateHud = false;                      //Updating HUD information
    public bool loadRandomMap = true;
    public bool ActivateNextMap = false;
    [Header("Audio controls")]
    public bool menuMusic = false;
    public bool inGameMusic = false;

    InputAction controlAction;
    InputAction pauseMenuAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
//        isPlaying = true;
        controlAction = InputSystem.actions.FindAction("Control");
        pauseMenuAction = InputSystem.actions.FindAction("PauseMenu");
//        StartGame();
        
    }


    // Update is called once per frame
    void Update()
    {
        //Control the GamePlay functions
        if (isPlaying && !isGameOver)                   //If isPlaying == TRUE and NOT GameOver, do something
        {
            updateHud = true;

        }
        else                                            //isPlaying == FALSE and/or GameOver == TRUE, do something
        {
            updateHud= false;
        }

        if (isPaused && !isGameOver)                    //If isPaused == TRUE, do something
        {
            updateHud = false;
            isPlaying = false;
        }

        if (controlAction.IsPressed())
        {
//            QuitGame();
            EnterMainMenu();
        }
        if (pauseMenuAction.IsPressed())
        {
            isPaused = true;
        }


        if (StatsManager.Instance.playerXDead)
        {
            IsGameOver();
        }
        
        if (ActivateNextMap)
        {
            BeginNextLevel();
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(true); // Re-activate this object after a new scene is loaded
    }

    public void StartGame()                             //Use method when first time starting game
    {
        //Every action needed for game to begin correctly
        SceneController.Instance.LoadSpecificLevel("PlayScene", OnPlaySceneLoaded);    //Check if PlayScene is active / Load if different scene
                                                                    //        SceneController.Instance.LoadPlayScene();
//        StartCoroutine(TimeDelay());


        //Call SceneController method
    }

    private void OnPlaySceneLoaded()
    {
        LevelManager.Instance.OnGameBegin();
        SpawnManager.Instance.LoadLevelSpawnPoints();
        //        StartCoroutine(DelaydStart());                  //Load level spawnpoints after delay, making sure scene is loaded
        isGameOver = false;
        isPlaying = true;
        updateHud = true;

    }

    private IEnumerator TimeDelay()
    {
        yield return new WaitForSeconds(2);
    }
    private IEnumerator DelaydStart()
    {
        Debug.Log("Entering DelaydStart");

        yield return new WaitForSeconds(1);
        SpawnManager.Instance.LoadLevelSpawnPoints();
        StopCoroutine(DelaydStart());
    }

    public void IsGameOver()
    {
        SpawnManager.Instance.StopSpawning();
        SpawnManager.Instance.spawningAllowed = false;
        isGameOver= true;
        isPlaying = false;
        updateHud = false;
        ActivateNextMap = true;
    }

    public void BeginNextLevel()
    {
        //Every action needed for next level to begin correctly
        SpawnManager.Instance.ClearSpawns();
        LevelManager.Instance.OnGameBegin();
        StartCoroutine(DelaydStart());
        StatsManager.Instance.ResetPlayerStats();       //Reset everything else but TotalScore for each player
//        LevelManager.Instance.InstantiateHUD();
        isGameOver = false;
        isPlaying = true;
        updateHud = true;
        StatsManager.Instance.playerXDead = false;

        ActivateNextMap = false;
    }

    public void EndLevel()                              //When level ends, do these functions
    {
        //Every Action needed for changing next level
        isGameOver = false;
        isPlaying = false;
        updateHud = false;
        
//        SpawnManager.Instance.onceDone = false;

        //Method propably Ending to StartGame();
        //Or method BeginNextLevel();
        //StartGame();
        //BeginNextLevel();
    }

    public void GamePaused()                            //Enter PauseState
    {
        isPlaying = false;
        updateHud = false;
    }

    public void ExitPauseState()                        //Exit PauseState
    {
        isPaused = false;
        isPlaying = true;
        updateHud = true;
    }

    public void EnterMainMenu()
    {
        //EndLevel();
//        SceneController.Instance.LoadSpecificLevel(MainMenu);
        QuitGame();
    }
    public static void QuitGame()
    {
        Debug.Log("Quit Game called");
//            if (Application.isPlaying)
                Application.Quit();
    }

}
