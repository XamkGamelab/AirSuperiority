using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private GameObject spawnManagerPrefab;
    [SerializeField] private GameObject levelManagerPrefab;
    [SerializeField] private GameObject sceneControllerPrefab;
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject statsManagerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //Ensure MonoBehaviour singletons exist
        EnsureSingleton(spawnManagerPrefab);
        EnsureSingleton(levelManagerPrefab);
        EnsureSingleton(sceneControllerPrefab);
        EnsureSingleton(gameManagerPrefab);
        EnsureSingleton(statsManagerPrefab);

        //Scene to load after singleton setup
        SceneManager.LoadScene("Level1");
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