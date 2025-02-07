using UnityEngine;

//Note: This singleton script is Non-MonoBehaviour and does not support Unity features i.e. Update()
public class ScoreManager
{
    private static ScoreManager _instance;
    public static ScoreManager Instance => _instance ??= new ScoreManager();

    private ScoreManager() { }      //Private constructor
    
    public int sum(int a, int b)
    {
        return a + b;
    }

    
    public int numberVariable = 100;

}
