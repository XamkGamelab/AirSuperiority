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
    [SerializeField] private TextMeshProUGUI healthValue0;
    [SerializeField] private TextMeshProUGUI healthValue1;
    [SerializeField] private TextMeshProUGUI shieldValue0;
    [SerializeField] private TextMeshProUGUI shieldValue1;

    [SerializeField] private TextMeshProUGUI playerNameText0;
    [SerializeField] private TextMeshProUGUI playerNameText1;
    [SerializeField] private TextMeshProUGUI scoreText0;
    [SerializeField] private TextMeshProUGUI scoreText1;
    [SerializeField] private TextMeshProUGUI ammoCount0;
    [SerializeField] private TextMeshProUGUI ammoCount1;
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

            //Setting Player names
            playerNameText0.text = ($"{StatsManager.Instance.player[0].name}");
            playerNameText1.text = ($"{StatsManager.Instance.player[1].name}");

            //Score
            scoreText0.text = ($"Score: {StatsManager.Instance.player[0].Score}");
            scoreText1.text = ($"Score: {StatsManager.Instance.player[1].Score}");

            //Updating sliders
            healthSlider0.value = StatsManager.Instance.player[0].Health;
            healthSlider1.value = StatsManager.Instance.player[1].Health;

            shieldSlider0.value = StatsManager.Instance.player[0].Shield;
            shieldSlider1.value = StatsManager.Instance.player[1].Shield;

            healthValue0.text = ($"{StatsManager.Instance.player[0].Health}/100");
            healthValue1.text = ($"{StatsManager.Instance.player[1].Health}/100");

            shieldValue0.text = ($"{StatsManager.Instance.player[0].Shield}/100");
            shieldValue1.text = ($"{StatsManager.Instance.player[1].Shield}/100");

            //Updating gun sprites and ammocount
            currentGunSprite0.sprite = StatsManager.Instance.player[0].CurrentGun.gunSprite;
            currentGunSprite1.sprite = StatsManager.Instance.player[1].CurrentGun.gunSprite;

            ammoCount0.text = ($"{StatsManager.Instance.player[0].CurrentGun.AmmoCount}");
            ammoCount1.text = ($"{StatsManager.Instance.player[1].CurrentGun.AmmoCount}");

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
