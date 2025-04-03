using UnityEngine;
using UnityEngine.Tilemaps;

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
    [SerializeField] private Canvas HUD;

    [SerializeField] private GameObject[] mapsToLoad;
    public GameObject map;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadMaps();
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
        //        SceneController.Instance.LoadSpecificLevel("PlayScene");
//        ClearLevel();
        InstantiateMAP();
//        InstantiateHUD();

/*
        if (GameManager.Instance.loadRandomMap)             //If randomMap loading Active
        {
            SceneController.Instance.LoadRandomMap();
        }
        //Load Level before SpawnPoints
//        SpawnManager.Instance.LoadLevelSpawnPoints();     //Use this if boolean controlled spawnPoint loading is not working
*/
    }

    public void ClearLevel()
    {
//        DestroyActiveHud();
        DestroyActiveMap();
    }
    private void LoadMaps()
    {
        mapsToLoad = Resources.LoadAll<GameObject>("Prefabs/Maps");
    }
    public void InstantiateMAP()
    {
        int ran = Random.Range(0, mapsToLoad.Length - 1);
//        int ran = 1;
        //        map = mapsToLoad[0];
        Instantiate(mapsToLoad[ran]);
    }
    public void InstantiateHUD()
    {
        Instantiate(HUD);
    }

    public void DestroyActiveMap()
    {
        if (map == null) return;

        Destroy(map);
    }

    public void DestroyActiveHud()
    {
        if (HUD ==  null) return;

        Destroy(HUD);
    }
}
