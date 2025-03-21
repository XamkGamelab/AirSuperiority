using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
 * 
 On player death, onplayerDeath(int x)

Player 1 score, GetScore(int 1)

 * 
 * 
 * 
 */


public class GameLoader : MonoBehaviour
{
    [SerializeField] private GameObject spawnManagerPrefab;
    [SerializeField] private GameObject levelManagerPrefab;
    [SerializeField] private GameObject sceneControllerPrefab;
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject statsManagerPrefab;
    [SerializeField] private GameObject GunManagerPrefab;
    [SerializeField] private GameObject audioControllerPrefab;
//    [SerializeField] private GameObject GunDataPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //Ensure MonoBehaviour singletons exist
        //        EnsureSingleton(GunDataPrefab);
        EnsureSingleton(gameManagerPrefab);
        EnsureSingleton(statsManagerPrefab);
        EnsureSingleton(GunManagerPrefab);
        EnsureSingleton(sceneControllerPrefab);
        EnsureSingleton(levelManagerPrefab);
        EnsureSingleton(spawnManagerPrefab);
        EnsureSingleton(audioControllerPrefab);

        //Scene to load after singleton setup
        //SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("TestLevel");
    }

    private void EnsureSingleton(GameObject prefab)
    {
        if (Object.FindFirstObjectByType(prefab.GetComponent<MonoBehaviour>().GetType()) == null)
        {
            Instantiate(prefab);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}