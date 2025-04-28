using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject creditsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (GameManager.Instance.endingGame)
        {
            creditsPanel.SetActive(true);
        }
        */
    }
    public void EnableInfo()
    {
        infoPanel.SetActive(true);
    }

    public void DisableInfo()
    {
        infoPanel.SetActive(false);
    }

    public void EnableCredits()
    {
        creditsPanel.SetActive(true);
    }
    public void callGameManagerQuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
