using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUDController : MonoBehaviour
{

    private bool updatingHud = false;
    public Slider healthSlider0;
    public Slider shieldSlider0;
    public Slider healthSlider1;
    public Slider shieldSlider1;

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image currentGunSprite0;
    [SerializeField] private Image currentGunSprite1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider0.maxValue = StatsManager.Instance.player[0].Health;
        healthSlider1.maxValue = StatsManager.Instance.player[1].Health;

        shieldSlider0.maxValue = StatsManager.Instance.player[0].Shield;
        shieldSlider0.maxValue = StatsManager.Instance.player[1].Shield;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.updateHud && GameManager.Instance.isPlaying) //checking !updatingHud only to avoid performing loop on every update.
        {
            //Start Coroutine for updating Hud
            updatingHud = true;
            Debug.Log("Updating HUD...");

            //Updating sliders
            healthSlider0.value = StatsManager.Instance.player[0].Health;
            healthSlider1.value = StatsManager.Instance.player[1].Health;

            shieldSlider0.value = StatsManager.Instance.player[0].Shield;
            shieldSlider1.value = StatsManager.Instance.player[1].Shield;

            //Updating gun sprites and ammocount
            currentGunSprite0.sprite = StatsManager.Instance.player[0].CurrentGun.gunSprite;
            currentGunSprite1.sprite = StatsManager.Instance.player[1].CurrentGun.gunSprite;

        }
        else if (!GameManager.Instance.updateHud && GameManager.Instance.isPaused)
        {
            //Stop Coroutine for updating Hud
            updatingHud = false;
        }

        if (GameManager.Instance.isPaused)
        {
            EnterPauseMenu();
        }
    }

    void EnterPauseMenu()
    {
        
    }


    //Here will be coroutine for Updating Hud
}
