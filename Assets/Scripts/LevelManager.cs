using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

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

    [Header("SpawnManager controls")]                   //Control spawning  [NOTE: spawnActive activates spawning for everything]
    public bool spawnActive = false;                    //Spawn everything (
    public bool spawnItemActive = false;                //Spawn Items       [NOTE: spanactive == false, spawnItemActive == true => Only items spawns]
    public bool spawnGunActive = false;                 //Spawn Guns

    private bool spawning = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPlaying && !spawning)
        {
            spawnActive = true;
            spawning = true;
        }

        if (spawning && !GameManager.Instance.isPlaying) 
        {
            spawnActive = false;
            spawning = false;
        }
    }

    public void OnGameBegin()
    {
        if (GameManager.Instance.loadRandomMap)
        {
            SceneController.Instance.LoadRandomMap();
        }
        //Load Level before SpawnPoints
//        SpawnManager.Instance.LoadLevelSpawnPoints();     //Use this if boolean controlled spawnPoint loading is not working

    }

    public void InstantiateHUD()
    {

    }
}
