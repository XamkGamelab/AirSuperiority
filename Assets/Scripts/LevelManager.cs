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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
//        Debug.Log($"Levelmanager informs value from scoreManager {ScoreManager.Instance.numberVariable}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
