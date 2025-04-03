using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    [SerializeField] public bool sceneReady = false;
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScenesFromBuildSettings();
    }

    private List<string> availableLevels = new List<string>();
    public string currentLevel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
//        LoadRandomMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadScenesFromBuildSettings()
    {
        availableLevels.Clear();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            //Exclude non-playable scenes
            if (sceneName != "GameLoader")
            {
                availableLevels.Add(sceneName);
                Debug.Log($"Added {sceneName} to {scenePath}");
            }
        }

        Debug.Log($"Loaded {availableLevels.Count} playable Levels.");
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void LoadRandomMap()
    {
        if (availableLevels.Count == 0)
        {
            Debug.LogWarning("No available Levels to Load!");
            return;
        }
        string nextLevel = availableLevels[Random.Range(0, availableLevels.Count)];
        currentLevel = nextLevel;
        Debug.Log($"Opening {nextLevel} scene");
        SceneManager.LoadScene(nextLevel);
    }

    public void LoadSpecificLevel(string levelName)
    {
        sceneReady = false;
        Debug.Log($"Trying to load level: {levelName}");
        if (availableLevels.Contains(levelName))
        {
            if (SceneManager.GetActiveScene().name == levelName)
            {
                return;
            }
            else
            {
                currentLevel = levelName;
                //                SceneManager.LoadScene(levelName);
                StartCoroutine(LoadSceneAsync(levelName));
            }
        }
        else
        {
            Debug.LogError($"LevelName {levelName} not found from list!");
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Prevents scene activation until it's fully loaded

        while (!asyncLoad.isDone)
        {
            // Check if the scene has finished loading (progress reaches 0.9)
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("Scene is loaded and ready!");
                asyncLoad.allowSceneActivation = true; // Now activate the scene
                sceneReady = true;
            }
            yield return null;
        }
    }
}
