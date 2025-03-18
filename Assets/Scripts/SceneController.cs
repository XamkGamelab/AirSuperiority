using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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
        LoadRandomMap();
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
            }
        }

        Debug.Log($"Loaded {availableLevels.Count} playable Levels.");
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
        if (!availableLevels.Contains(levelName))
        {
            currentLevel = levelName;
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogError("LevelName not found from list!");
        }
    }
}
