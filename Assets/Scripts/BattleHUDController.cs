using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUDController : MonoBehaviour
{

    [SerializeField] private bool updatingHud = false;
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
    [SerializeField] private TextMeshProUGUI gameTime;
    [SerializeField] private TextMeshProUGUI player1Victories;
    [SerializeField] private TextMeshProUGUI player2Victories;
    [SerializeField] private TextMeshProUGUI player1totalScore;
    [SerializeField] private TextMeshProUGUI player2totalScore;
    [SerializeField] private TextMeshProUGUI totalTime;
    [SerializeField] private TextMeshProUGUI matchTime;
    [SerializeField] private Image currentGunSprite0;
    [SerializeField] private Image currentGunSprite1;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject kamiKazeInfoPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider0.maxValue = StatsManager.Instance.player[0].Health;
        healthSlider1.maxValue = StatsManager.Instance.player[1].Health;

        shieldSlider0.maxValue = StatsManager.Instance.player[0].Shield;
        shieldSlider0.maxValue = StatsManager.Instance.player[1].Shield;
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        kamiKazeInfoPanel.SetActive(false);
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
        if (kamiKazeInfoPanel.activeSelf && !GameManager.Instance.updateHud)
        {
            DeactKamikaze();
        }

        if (GameManager.Instance.kamikazeActivation < StatsManager.Instance.GetPlayTime() && GameManager.Instance.kamikazeActivation + 5f > StatsManager.Instance.GetPlayTime())
        {
            Debug.Log("Activate kamikaze info panel");
            kamiKazeInfoPanel.SetActive(true);
        }
        else if (GameManager.Instance.kamikazeActivation + 5f < StatsManager.Instance.GetPlayTime() && kamiKazeInfoPanel.activeSelf)
        {
            Debug.Log("DeActivate kamikaze info panel");
            kamiKazeInfoPanel.SetActive(false);
        }

        if (gameOverMenu.activeSelf  == true && !GameManager.Instance.isGameOver)
        {
            gameOverMenu.SetActive(false);
            Cursor.visible = false;
        }
        if (GameManager.Instance.updateHud && GameManager.Instance.isPlaying) //checking !updatingHud only to avoid performing loop on every update.
        {
            //Start Coroutine for updating Hud
            updatingHud = true;
            //Debug.Log("Updating HUD...");

            //Check if player[0] HealthSlider MAX value is correct. If not, assume none slider has been set yet.
            if (healthSlider0.maxValue != StatsManager.Instance.player[0].Health && StatsManager.Instance.GetPlayTime() < 2)
            {
                healthSlider0.maxValue = StatsManager.Instance.player[0].Health;
                healthSlider1.maxValue = StatsManager.Instance.player[1].Health;
                shieldSlider0.maxValue = StatsManager.Instance.player[0].Shield;
                shieldSlider0.maxValue = StatsManager.Instance.player[1].Shield;

            }

            //Setting Player names
            playerNameText0.text = ($"{StatsManager.Instance.player[0].name}");
            playerNameText1.text = ($"{StatsManager.Instance.player[1].name}");

            //Score
            scoreText0.text = ($"Score: {StatsManager.Instance.player[0].Score}");
            scoreText1.text = ($"Score: {StatsManager.Instance.player[1].Score}");

            //Updating sliders
            healthSlider0.value = StatsManager.Instance.player[0].Health;
            healthSlider1.value = StatsManager.Instance.player[1].Health;
            Debug.Log($"HealthSliderValue Player1: {healthSlider0.value}");
            Debug.Log($"HealthSliderValue Player2: {healthSlider1.value}");

            shieldSlider0.value = StatsManager.Instance.player[0].Shield;
            shieldSlider1.value = StatsManager.Instance.player[1].Shield;

            healthValue0.text = ($"{StatsManager.Instance.player[0].Health}/100");
            healthValue1.text = ($"{StatsManager.Instance.player[1].Health}/100");
            Debug.Log($"HealthSliderValue text Player1: {healthValue0.text}");
            Debug.Log($"HealthSliderValue text Player2: {healthValue1.text}");

            shieldValue0.text = ($"{StatsManager.Instance.player[0].Shield}/100");
            shieldValue1.text = ($"{StatsManager.Instance.player[1].Shield}/100");

            //Updating gun sprites and ammocount
            currentGunSprite0.sprite = StatsManager.Instance.player[0].CurrentGun.gunSprite;
            currentGunSprite1.sprite = StatsManager.Instance.player[1].CurrentGun.gunSprite;

            ammoCount0.text = ($"{StatsManager.Instance.player[0].CurrentGun.AmmoCount}");
            ammoCount1.text = ($"{StatsManager.Instance.player[1].CurrentGun.AmmoCount}");

            //Updating victory counter
            player1Victories.text = ($"{StatsManager.Instance.player[0].Victories}");
            player2Victories.text = ($"{StatsManager.Instance.player[1].Victories}");

            //Game time update
            int totalgameTime = Mathf.FloorToInt(StatsManager.Instance.GetPlayTime());
            int minutes = totalgameTime / 60;
            int seconds = totalgameTime % 60;
            gameTime.text = ($"Game time: {minutes:D2}:{seconds:D2}");


        }
        else if (!GameManager.Instance.updateHud && GameManager.Instance.isPaused)
        {
            //Stop Coroutine for updating Hud
            updatingHud = false;
        }

        if (GameManager.Instance.isPaused && !pauseMenu.activeSelf)
        {
            EnterPauseMenu();
        }

        if (GameManager.Instance.isGameOver && !gameOverMenu.activeSelf == true && GameManager.Instance.menuElementsVisible)
        {
            GameOver();
        }
        if (gameOverMenu.activeSelf == true && updatingHud && !GameManager.Instance.isGameOver)
        {
            gameOverMenu.SetActive(false);
            Cursor.visible = false;
        }
        

    }

    private void EnterPauseMenu()
    {
        //Actions to do when entering pause menu. PauseMenu overlay etc.
        //Some button or shit to call exit pause menu method after continue

        pauseMenu.SetActive(true);

    }

    public void ExitPauseMenu()
    {
        pauseMenu.SetActive(false);
        DeactKamikaze();   
        GameManager.Instance.ExitPauseState();
    }

    public void newGame()
    {
        DeactKamikaze();
        gameOverMenu.SetActive(false);
        GameManager.Instance.ActivateNextLevel();
    }
    
    private void GameOver()
    {
        DeactKamikaze();
        Cursor.visible = true;
        gameOverMenu.SetActive(true);
        player1totalScore.text = ($"Player 1 total score: {StatsManager.Instance.player[0].TotalScore}");
        player2totalScore.text = ($"Player 2 total score: {StatsManager.Instance.player[1].TotalScore}");
        int totaltime = Mathf.FloorToInt(StatsManager.Instance.GetTotalPlayTime());
        int minutes = totaltime / 60;
        int seconds = totaltime % 60;
        totalTime.text = ($"Total game time: {minutes:D2}:{seconds:D2}");

        int levelTime = Mathf.FloorToInt(StatsManager.Instance.GetPlayTime());
        int minutes2 = levelTime / 60;
        int seconds2 = levelTime % 60;
        matchTime.text = ($"Level time: {minutes2:D2}:{seconds2:D2}");
    }
    

    public void MainMenu()
    {
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        GameManager.Instance.EnterMainMenu();
    }

    private void DeactKamikaze()
    {
        kamiKazeInfoPanel.SetActive(false);
    }

    //Here will be coroutine for Updating Hud
}
