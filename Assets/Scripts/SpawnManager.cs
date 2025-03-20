using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

/************************************************************************************************************
 *                                          INFORMATION 
 ************************************************************************************************************
 *  SpawnManager is controlled through LevelManager. Three booleans control either item- or gunSpawning or
 *  sets both active:
 *  spawnActive = true      => Item and gun are spawning
 *  spawnItemActive = true  => Only items are spawning
 *  spawnGunActive = true   => Only guns are spawning
 *  
 *  itemSpawnRate is a factor for randomizing time when spawning to happen. Minumum value is factor * 0.8 and
 *  maximun value can be 1.6 x factor
 * 
 *  gunSpawnRate is a factor for randomizing time when spawning to happen. Minumum value is factor * 0.8 and
 *  maximun value can be 1.6 x factor
 * 
 * --------------------
 * onceDone bool controls loading current map spawnpoints. Bool must be set to false when changing level
 * to enable new level spawnpoit information loading. GameManager can control this boolean.
 * 
 * 
 */

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public string tilemapObjectName = "PlayArea;";          //Tilemap area where spawning is possible
    public LayerMask LevelElementLayer;                     //LayerMask for LevelElement collision detection
    public string resourcesFolder1 = "Prefabs/SpawnItems";  //Resources folder for spawnable items
    public string resourcesFolder2 = "Prefabs/Guns";        //Resources folder for spawnable guns

    private Tilemap playAreaTilemap;
    private BoundsInt spawnBounds;
    private GameObject[] itemsToSpawn;
    private GameObject[] gunsToSpawn;
    private List<Vector3> validSpawnPositions = new List<Vector3>();
    [Header("Spawning variables")]
    private float itemSpawnRate = 1f;                     //Define Item spawnRate
    private float gunSpawnRate = 1f;                      //Define Gun spawnRate
    [SerializeField] private bool spawningItems = false;
    [SerializeField] private bool spawningGuns = false;

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

                                                            //Load Spawnable Objects
        LoadSpawnableObjects();
        LoadSpawnableGuns();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

        if (tilemapParent)
        {
                                                                        //Find correct (PlayArea) Tilemap inside parent
            foreach (Tilemap tilemap in tilemapParent.GetComponentsInChildren<Tilemap>())
            {
                if (tilemap.gameObject.layer == LayerMask.NameToLayer("PlayArea"))
                {
                    playAreaTilemap = tilemap;
                    spawnBounds = playAreaTilemap.cellBounds;
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
        validSpawnPositions.Clear();

        foreach (Vector3Int cellPosition in spawnBounds.allPositionsWithin)
        {
            if (playAreaTilemap.HasTile(cellPosition))                  //Check if the tile exist in the playArea
            {
                Vector3 worldPosition = playAreaTilemap.GetCellCenterWorld(cellPosition);
                if (!Physics2D.OverlapCircle(worldPosition, 0.3f, LevelElementLayer))   //avoid obstacles, 0.3f is radius of spawnable object
                {
                    validSpawnPositions.Add(worldPosition);
                }
            }
        }

        if (validSpawnPositions.Count == 0)
        {
            Debug.LogWarning("No valid spawn positions found!");
        }
    }
    public void StartSpawning()                                         //Begins Coroutines for item- and gunSpawning by GameManagers booleans 
    {
        if (!spawningItems && (LevelManager.Instance.spawnItemActive || LevelManager.Instance.spawnActive))
        {
//            Debug.Log($"Starting coroutine SpawnItemroutine ({spawningItems})");
            StartCoroutine(SpawnItemRoutine());                         //ItemSpawner
        }

        if (!spawningGuns && (LevelManager.Instance.spawnGunActive || LevelManager.Instance.spawnActive))
        {
            StartCoroutine(SpawnGunRoutine());                          //GunSpawner
        }
    }

    private IEnumerator SpawnItemRoutine()                              //ItemSpawner
    {
        spawningItems = true;

        while (LevelManager.Instance.spawnItemActive || LevelManager.Instance.spawnActive)
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

        while (LevelManager.Instance.spawnGunActive || LevelManager.Instance.spawnActive)
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
        if (!LevelManager.Instance.spawnActive && !LevelManager.Instance.spawnGunActive && !LevelManager.Instance.spawnItemActive)
        {
            StopAllCoroutines();
            spawningItems = false;
            spawningGuns = false;
        }
        else if (!LevelManager.Instance.spawnActive && !LevelManager.Instance.spawnItemActive)
        {
            StopCoroutine(SpawnItemRoutine());
            spawningItems = false;
        }
        else if (!LevelManager.Instance.spawnActive && !LevelManager.Instance.spawnGunActive)
        {
            StopCoroutine(SpawnGunRoutine());
            spawningGuns = false;
        }

    }
}
