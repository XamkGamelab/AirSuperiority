using UnityEngine;
using UnityEngine.UI;
using static StatsManager;

public class BattleHUDController : MonoBehaviour
{
    // UI elements for stats
    public Text scoreText;
    public Slider healthSlider;
    public Slider shieldSlider;
    public Text ammoText;
    public Text timeText;
    
    private bool updatingHud = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // UpdateHUD();
        // Calling for UpdateHUD() function
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

    //  private void UpdateHUD()
    //   {
    //      PlayerData playerData = StatsManager.Instance.player[0];
    //      scoreText.text = "Score: " + playerData.Score.ToString();
    //      healthSlider.value = playerData.Health / 100f;
    //      shieldSlider.value = playerData.Shield / 100f;
    //      ammoText.text = "Ammo: " + playerData.CurrentGun.AmmoCount.ToString();
    //      timeText.text = "Time: " + StatsManager.Instance.GetPlayTime().ToString("F2") + "s";
    //   }


    //Here will be coroutine for Updating Hud
}
