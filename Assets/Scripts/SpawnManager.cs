using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public string tilemapObjectName = "PlayArea;";          //Tilemap area where spawning is possible
    public LayerMask levelElementLayer;                     //LayerMask for LevelElement collision detection
    public string resourcesFolder1 = "Prefabs/SpawnItems";  //Resources folder for spawnable items
    public string resourcesFolder2 = "Prefabs/Guns";        //Resources folder for spawnable guns

    private Tilemap playAreaTilemap;
    private BoundsInt spawnBounds;
    private GameObject[] itemsToSpawn;
    private GameObject[] gunsToSpawn;
    private List<Vector3> validSpawnPositions = new List<Vector3>();

    private bool onceDone = false;

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
        //        LoadLevelSpawnPoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPlaying && !onceDone)
        {
            LoadLevelSpawnPoints();
            SpawnGun();
            onceDone = true;
        }
    }

    public void LoadLevelSpawnPoints()                                  //Call for level spawnPoint Initialization
    {
        Debug.Log("Begin loading spawnpoints.");
        FindPlayAreaBounds();
        CollectValidSpawnPositions();
    }
    private void LoadSpawnableObjects()
    {
        itemsToSpawn = Resources.LoadAll<GameObject>(resourcesFolder1);
        if (itemsToSpawn.Length == 0)
            Debug.LogWarning($"No spawnable items found in Resources/{resourcesFolder1}");
    }

    private void LoadSpawnableGuns()
    {
        gunsToSpawn = Resources.LoadAll<GameObject>(resourcesFolder2);
        if (itemsToSpawn.Length == 0)
            Debug.LogWarning($"No spawnable guns found in Resources/{resourcesFolder2}");
    }

    private void FindPlayAreaBounds()
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

    private void CollectValidSpawnPositions()
    {
        validSpawnPositions.Clear();

        foreach (Vector3Int cellPosition in spawnBounds.allPositionsWithin)
        {
            if (playAreaTilemap.HasTile(cellPosition))                  //Check if the tile exist in the playArea
            {
                Vector3 worldPosition = playAreaTilemap.GetCellCenterWorld(cellPosition);
                if (!Physics2D.OverlapCircle(worldPosition, 0.3f, levelElementLayer))   //avoid obstacles, 0.3f is radius of spawnable object
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
    public void SpawnItem()
    {
        if (validSpawnPositions.Count == 0 || itemsToSpawn.Length == 0)
            return;

        Vector3 spawnPosition = validSpawnPositions[Random.Range(0, validSpawnPositions.Count)];
        Instantiate(itemsToSpawn[Random.Range(0, itemsToSpawn.Length)], spawnPosition, Quaternion.identity);
    }

    public void SpawnGun()
    {
        if (validSpawnPositions.Count == 0 || gunsToSpawn.Length == 0)
            return;

        Vector3 spawnPosition = validSpawnPositions[Random.Range(0, validSpawnPositions.Count)];

        Debug.Log($"Spawning at: {spawnPosition}");

        GameObject spawnedGun = Instantiate(gunsToSpawn[Random.Range(0, gunsToSpawn.Length)], spawnPosition, Quaternion.identity);

        spawnedGun.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
    }

}
