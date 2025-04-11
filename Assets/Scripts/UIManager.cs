using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject infoPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableInfo()
    {
        infoPanel.SetActive(true);
    }

    public void DisableInfo()
    {
        infoPanel.SetActive(false);
    }
}
