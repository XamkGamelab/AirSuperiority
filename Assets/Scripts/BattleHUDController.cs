using UnityEngine;

public class BattleHUDController : MonoBehaviour
{

    private bool updatingHud = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.updateHud && !updatingHud) //checking !updatingHud only to avoid performing loop on every update.
        {
            //Start Coroutine for updating Hud
            updatingHud = true;
        }
        else if (!GameManager.Instance.updateHud)
        {
            //Stop Coroutine for updating Hud
            updatingHud = false;
        }
    }

    //Here will be coroutine for Updating Hud
}
