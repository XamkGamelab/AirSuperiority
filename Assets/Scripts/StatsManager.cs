using UnityEngine;
using System.Collections;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

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

    /* Methods that can be called:
     * StatsManager.Instance.StartCountTimeCoroutine()      Begins counting time    (If routine is stopped and needs to be started again)
     * StatsManager.Instance.StopCounttime()                Ends counting time      (Propably no need to stop coroutine on normal situation)
     * StatsManager.Instance.GetPlayTime()                  Get current playing time (Returns float, Time played)
     * StatsManager.Instance.AffectPlayer()                 Affect player statistics. AddScore, TakeDamage, ConsumeShield, ChangeGun
     * Usage: StatsManager.Instance.AffectPlayer(0, Action i.e "AddScore", 10) Result: Add 10 to player 0 score
     * StatsManager.Instance.ResetPlayerStats()             Resets EVERY Player statistics, excluding TotalScore
     * 
     * Arrays with information:
     * StatsManager.Instance.player[x]                      Read or affect PlayerData (Score, TotalScore, Health, Shield, CurrentGun)
     * StatsManager.Instance.gun[x]                         Read or affect GunData. (GunName, FireRate, AmmoCount, Ammonition)
     * 
     */

    //For time measuring
    public float startTime = 0;
    public float stopTime = 0;
    public float timeInterval = 0;
    public bool calculateTimeRef = false;

    //Players information
    public PlayerData[] player = new PlayerData[2];         //Changing PlayerData[x] changes active player count

    //Gun information
    public GunData[] gun = new GunData[3];

                                                            
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCountTimeCoroutine();                          //Start time counting coroutine

        for (int i = 0; i < player.Length; i++)
        {
            player[i] = new PlayerData();                   //Initialize each PlayerData instance
        }

//        ReadUsableGuns();                                 //Initialize all guns

    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    public class PlayerData                                 //PlayerData that can be used
    {
        public int Score = 0;
        public int TotalScore = 0;
        public float Health = 100f;
        public float Shield = 100f;
        public int CurrentGun = 0;
    }

    [System.Serializable]
    public class GunData                                    //GunData that can be used. Read values from GunPrefabs marked with layer or tag
    {
        public string GunName = "DefaultGun";
        public float FireRate = 1;                          //Variable could be for example tide to deltaTime
        public float AmmoCount = 5;
        public float Ammonition = 0;                        //Defines what kind of a projectile gun shoots 
                    //This needs to be defined. 0 == projectile, 1 == laserTypeBeam, 2 == ObstacleCreator, etc.
    }

    public void StartCountTimeCoroutine()                   //Begin Time measuring coroutine
    {
        StartCoroutine(CountTimeCoroutine());
    }

    public void StopCountTime()                             //Stop Time measuring coroutine
    {
        StopCoroutine(CountTimeCoroutine());
    }

    IEnumerator CountTimeCoroutine()                        //Time measuring coroutine
    {
        if (GameManager.Instance.isPlaying != calculateTimeRef)
        {
            startTime = Time.unscaledDeltaTime;             //Get start time
            calculateTimeRef = true;

        }
        else
        {
            stopTime = Time.unscaledDeltaTime;              //Get stop time
            calculateTimeRef = false;

        }

        timeInterval = Time.unscaledDeltaTime - startTime;  //Elapsed time

        yield return new WaitForSeconds(1.0f);
    }

    public float GetPlayTime()                              //Get elapsed time
    {
        return timeInterval;
    }

    public void AffectPlayer(int playerIndex, string action, float value)   //Affect player statistics
    {
        if (playerIndex < 0 || playerIndex >= player.Length) return;

        switch (action)
        {
            case "AddScore":                                //Add/substract score by amount of value
                player[playerIndex].Score += (int)value;
                break;
            case "TakeDamage":                              //Negative value removes Health, positive adds health
                player[playerIndex].Health += value;
                break;
            case "ConsumeShield":                           //Negative value removes shield, positive value adds shield
                player[playerIndex].Shield += value;
                break;
            case "ChangeGun":                               //Change gun ID
                player[playerIndex].CurrentGun = (int)value;
                break;
        }
    }

    public void ResetPlayerStats()                      //Reset every player stats, Exclude reseting TotalScore
    {
        for (int i = 0; i < player.Length; i++)
        {
            player[i].Score = 0;
            player[i].Health = 100;
            player[i].Shield = 100;
            player[i].CurrentGun = 0;
        }

    }

    public void ReadUsableGuns()        //This will propably be moved to GameLoader
    {
        //Everything needed to find GunPrefabs, read information and add information to GunData[x]
        //How? Prefabs has layer "GUN", or tag "GUN", or something else...
    }


}
