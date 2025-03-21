using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public string tilemapObjectName = "PlayArea";          //Tilemap area where spawning is possible
    public LayerMask LevelElementLayer;                     //LayerMask for LevelElement collision detection
    public string resourcesFolder1 = "Prefabs/SpawnItems";  //Resources folder for spawnable items
    public string resourcesFolder2 = "Prefabs/Guns";        //Resources folder for spawnable guns

    private Tilemap playAreaTilemap;
    private BoundsInt spawnBounds;
    private GameObject[] itemsToSpawn;
    private GameObject[] gunsToSpawn;
    private List<Vector3> validSpawnPositions = new List<Vector3>();
    [Header("Spawning variables")]
    private float itemSpawnRate = 15f;                     //Define Item spawnRate
    private float gunSpawnRate = 10f;                      //Define Gun spawnRate
    [SerializeField] private bool spawningItems = false;
    [SerializeField] private bool spawningGuns = false;
    [SerializeField] public bool spawningAllowed = false;

    public bool onceDone = false;                           //Controls spawnpoint loading.

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
//        Debug.Log($"SpawnManager gives values 10 and 20 to scoreManagers sum method which returns: {ScoreManager.Instance.sum(20, 10)}");
    }

    // Update is called once per frame
    void Update()
    {

//        if (GameManager.Instance.isPlaying && !onceDone)                //This is done once
//        {
//            LoadLevelSpawnPoints();                                     //Load current level spawnPoints
//            SpawnGun();
//            onceDone = true;                                            //Control through GameManager when starting new map
//        }

        StartSpawning();

    }

    public void LoadLevelSpawnPoints()                                  //Call for level spawnPoint Initialization
    {
        Debug.Log("Begin loading spawnpoints.");

        FindPlayAreaBounds();
        CollectValidSpawnPositions();
        spawningAllowed = true;
    }
    private void LoadSpawnableObjects()                                 //Load Spawnable items from Resources folder
    {
        itemsToSpawn = Resources.LoadAll<GameObject>(resourcesFolder1);
        if (itemsToSpawn.Length == 0)
            Debug.LogWarning($"No spawnable items found in Resources/{resourcesFolder1}");
    }

    private void LoadSpawnableGuns()                                    //Load Spawnable guns from Resource folder
    {
        gunsToSpawn = Resources.LoadAll<GameObject>(resourcesFolder2);
        if (itemsToSpawn.Length == 0)
            Debug.LogWarning($"No spawnable guns found in Resources/{resourcesFolder2}");
    }

    private void FindPlayAreaBounds()                                   //Determine tilemap boundaries
    {
        GameObject tilemapParent = GameObject.Find("Tilemap");  //Search tilemap by layerName
        Debug.Log($"Found GameObject {tilemapParent.name}");

        if (tilemapParent)
        {
                                                                        //Find correct (PlayArea) Tilemap inside parent
            foreach (Tilemap tilemap in tilemapParent.GetComponentsInChildren<Tilemap>())
            {
                if (tilemap.gameObject.layer == LayerMask.NameToLayer("PlayArea"))
                {
                    playAreaTilemap = tilemap;
                    spawnBounds = playAreaTilemap.cellBounds;
                    Debug.Log($"Found bound values x: {spawnBounds.x} y: {spawnBounds.y}");
                    return;
                }
            }

            Debug.LogError($"No Tilemap found in PlayArea layer!");
        }
        else
        {
            Debug.LogError("Tilemap parent GameObject not found!");
        }
    }

    private void CollectValidSpawnPositions()                           //Determine valid spawnPoints
    {
        LevelElementLayer = LayerMask.GetMask("LevelElement");
        Debug.Log($"LevelElement LayerMask Value: {LevelElementLayer.value}");
        Debug.Log($"LayerMask for 'LevelElement': {1 << LayerMask.NameToLayer("LevelElement")}");

        validSpawnPositions.Clear();
        Debug.Log($"Valid SpawnPositions Cleared: {validSpawnPositions.Count}");
        string temp = LevelElementLayer.ToString();
        Debug.Log($"Using LayerMask {LevelElementLayer.value} for detecting overlap.");
        foreach (Vector3Int cellPosition in spawnBounds.allPositionsWithin)
        {
            if (playAreaTilemap.HasTile(cellPosition))                  //Check if the tile exist in the playArea
            {
                Vector3 worldPosition = playAreaTilemap.GetCellCenterWorld(cellPosition);
                if (!Physics2D.OverlapCircle(worldPosition, 0.5f, LevelElementLayer))   //avoid obstacles, 0.3f is radius of spawnable object
                {
                    validSpawnPositions.Add(worldPosition);

                    Collider2D hit = Physics2D.OverlapCircle(worldPosition, 0.3f, LevelElementLayer);
                    if (hit == null)
                    {
                        validSpawnPositions.Add(worldPosition);
                    }
                    else
                    {
                        Debug.Log($"Blocked by: {hit.gameObject.name} at {worldPosition}");
                    }
                }
            }
        }
        Debug.Log($"Valid SpawnPositions: {validSpawnPositions.Count}");
        if (validSpawnPositions.Count == 0)
        {
            Debug.LogWarning("No valid spawn positions found!");
        }
    }
    public void StartSpawning()                                         //Begins Coroutines for item- and gunSpawning by GameManagers booleans 
    {
        if (!spawningItems && GameManager.Instance.isPlaying && spawningAllowed && (LevelManager.Instance.spawnItemActive || LevelManager.Instance.spawnActive))
        {
//            Debug.Log($"Starting coroutine SpawnItemroutine ({spawningItems})");
            StartCoroutine(SpawnItemRoutine());                         //ItemSpawner
        }

        if (!spawningGuns && GameManager.Instance.isPlaying && spawningAllowed && (LevelManager.Instance.spawnGunActive || LevelManager.Instance.spawnActive))
        {
            StartCoroutine(SpawnGunRoutine());                          //GunSpawner
        }
    }

    private IEnumerator SpawnItemRoutine()                              //ItemSpawner
    {
        spawningItems = true;

        while (LevelManager.Instance.spawnItemActive || LevelManager.Instance.spawnActive && spawningAllowed && !GameManager.Instance.isGameOver && !StatsManager.Instance.playerXDead)
        {
            Debug.Log("Spawning items Coroutine");
            float itemWaitTime = Random.Range(itemSpawnRate * 0.8f, itemSpawnRate * 1.6f);      //Randomize spawnTime using itemSpawnRate factor
            yield return new WaitForSeconds(itemWaitTime);
            SpawnItem();
        }

        spawningItems = false;
    }
    public void SpawnItem()
    {
        if (validSpawnPositions.Count == 0 || itemsToSpawn.Length == 0)                         //Check that valid spawnpositions exist
            return;

        Debug.Log("Spawning Items");
        Vector3 spawnPosition = validSpawnPositions[Random.Range(0, validSpawnPositions.Count)];    //Randomize spanPosition

        Instantiate(itemsToSpawn[Random.Range(0, itemsToSpawn.Length)], spawnPosition, Quaternion.identity);
    }

    private IEnumerator SpawnGunRoutine()                               //GunSpawner
    {
        spawningGuns = true;

        while (LevelManager.Instance.spawnGunActive || LevelManager.Instance.spawnActive && spawningAllowed && !GameManager.Instance.isGameOver && !StatsManager.Instance.playerXDead)
        {

            float gunWaitTime = Random.Range(gunSpawnRate * 0.8f, gunSpawnRate * 1.6f);         //Randomize spawnTime using gunSpawnRate factor
            Debug.Log($"Spawning Guns Coroutine. Wait time {gunWaitTime}");
            yield return new WaitForSeconds(gunWaitTime);
            SpawnGun();
        }

        spawningGuns = false;
    }

    public void SpawnGun()
    {

        if (validSpawnPositions.Count == 0 || gunsToSpawn.Length == 0)                          //Check that valid spawnpositions exist
            return;
//        Debug.Log("Spawning Guns");
        Vector3 spawnPosition = validSpawnPositions[Random.Range(0, validSpawnPositions.Count)];    //Randomize spanPosition
        Debug.Log("Spawning guns");
        Debug.Log($"Spawning at: {spawnPosition}");

        GameObject spawnedGun = Instantiate(gunsToSpawn[Random.Range(0, gunsToSpawn.Length)], spawnPosition, Quaternion.identity);

        spawnedGun.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);       //Make sure spawning happens on right depth (this was partly for debugging)
    }

    public void StopSpawning()                                                                  //Stops spawner coroutines
    {
        if (!LevelManager.Instance.spawnActive && !LevelManager.Instance.spawnGunActive && !LevelManager.Instance.spawnItemActive && !GameManager.Instance.isPlaying)
        {
            StopAllCoroutines();
            spawningItems = false;
            spawningGuns = false;
        }
        else if (!LevelManager.Instance.spawnActive && !LevelManager.Instance.spawnItemActive && GameManager.Instance.isPlaying)
        {
            StopCoroutine(SpawnItemRoutine());
            spawningItems = false;
        }
        else if (!LevelManager.Instance.spawnActive && !LevelManager.Instance.spawnGunActive && GameManager.Instance.isPlaying)
        {
            StopCoroutine(SpawnGunRoutine());
            spawningGuns = false;
        }

    }
}
