using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

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
        
    }
}
