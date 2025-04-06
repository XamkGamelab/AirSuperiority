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
     * StatsManager.Instance.gun[x]        [DO NOT USE]                 Read or affect GunData. (GunName, FireRate, AmmoCount, Ammonition)
     *                                     Instead, use:
     *StatsManager.Instance.player[x].CurrentGun.xxxxxx     Includes all variables that gun has. These are variables that can and should be modified.
     *                                                      For example ammoCount is used to track remaining bullets.
     *                                                      GunData array on the other hand only keeps information of usable guns and their properties
     *                                                      and is accessed by using gunName. When changing gun, array is read for specific gun to get data
     *                                                      and then stored to PlayerData.CurrentGun.
     * 
     * Resources
     * public class Gundata
     *  var gd = Resources.LoadALL("Weapons", typeof(GunData)).ToList();
     *  
     *  List<GunData> guns = new List<GunData>();
     *  gd.ForEach(obj =>
     *  {
     *  var go = Instantiate<GameObject>(obj as GameObject).GetComponent<GunData>;
     *  });
     *  
     *  Guns[4].Shoot();
     *  
     *  
     */

    [Header("Time measuring")]    //For time measuring
    public float startTime = 0;
    public float stopTime = 0;
    public float timeInterval = 0;
    public float playTime = 0;
    public bool calculateTimeRef = false;

    [Header("Player information")]//Players information
    public PlayerData[] player = new PlayerData[2];         //Changing PlayerData[x] changes active player count
    [SerializeField] private string defaultGun = "BasicGun";
    [SerializeField] public bool playerXDead = false;
    [SerializeField] public string deadPlayerName;

    [Header("Constants")]
    public const float maxHealth = 100;
    public const float maxShield = 100;

    //Gun information
    //public GunData[] gun = new GunData[3];

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCountTimeCoroutine();                          //Start time counting coroutine
        Debug.Log($"CountTimeCoroutine started.");

        for (int i = 0; i < player.Length; i++)
        {
            player[i] = new PlayerData();                   //Initialize each PlayerData instance           
            player[i].name = $"Player{i}";
        }
        LoadDefaultGunData();
//        ReadUsableGuns();                                 //Initialize all guns

    }

    // Update is called once per frame
    void Update()
    {
        if (playerXDead)
        {
            deadPlayerName = WhichPlayerIsDead();
        }
    }

    [System.Serializable]
    public class PlayerData                                 //PlayerData that can be used
    {
        public string name = "Player";
        public int Score = 0;
        public int TotalScore = 0;
        public int Victories = 0;
        public float Health = maxHealth;
        public float Shield = maxShield;
        public GunData CurrentGun;                          //Gundata for CurrentGun inside PlayerData
        public bool playerDead = false;
    }

    public void LoadDefaultGunData()
    {
        for (int i = 0; i < player.Length; i++)
        {
            player[i].CurrentGun = GunManager.Instance.GetGunData(defaultGun);   //Get GunData by default gun name
            Debug.Log($"Default Gun for Player {i} is {player[i].CurrentGun.GunName}");
        }
    }

    public void EquipGun(string newGun, int PlayerIndex)
    {
        GunData gun = GunManager.Instance.GetGunData(newGun);   //Get currentGun data by name from array
        player[PlayerIndex].CurrentGun = gun;               //Set received currentGun data to PlayerData
    }

    /*
    [System.Serializable]
    public class GunData                                    //GunData that can be used. Read values from GunPrefabs marked with layer or tag
    {
        public string GunName = "DefaultGun";
        public float FireRate = 1;                          //Variable could be for example tide to deltaTime
        public float AmmoCount = 5;
        public float Ammonition = 0;                        //Defines what kind of a projectile gun shoots
        public float speed = 5f;                            //Bullet flying speed
        public float destroyTime = 3f;                      //Time before bullet is destroyed

        //This needs to be defined. 0 == projectile, 1 == laserTypeBeam, 2 == ObstacleCreator, etc.
    }
    */

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

        if (GameManager.Instance.isPlaying != calculateTimeRef && !GameManager.Instance.isPaused)
        {
            startTime = Time.unscaledDeltaTime;             //Get start time
            calculateTimeRef = true;

        }
        else
        {
            stopTime = Time.unscaledDeltaTime;              //Get stop time
            calculateTimeRef = false;

        }

        if (GameManager.Instance.isPaused && !calculateTimeRef)
        {
            playTime += timeInterval;

            while (GameManager.Instance.isPaused)
            {
                yield return new WaitForSeconds(1.0f);
            }

            calculateTimeRef = true;
        }

        timeInterval = Time.unscaledDeltaTime - startTime;  //Elapsed time
        
        yield return new WaitForSeconds(1.0f);
    }


    public float GetPlayTime()                              //Get elapsed time
    {
        return timeInterval + playTime;
    }

    public void AffectPlayer(int playerIndex, string action, float value)   //Affect player statistics
    {
        if (playerIndex < 0 || playerIndex >= player.Length) return;

        switch (action)
        {
            case "AddScore":                                //Add/substract score by amount of value
                player[playerIndex].Score += (int)value;
                Debug.Log($"Player{playerIndex} score is: {player[playerIndex].Score}");
                if (player[playerIndex].TotalScore < player[playerIndex].Score)
                    player[playerIndex].TotalScore += (int)value;
                break;
            case "TakeDamage":
                {
                    float remainingDamage = -value; // Assume value is negative for damage

                    // Apply damage to shield
                    if (player[playerIndex].Shield > 0)
                    {
                        float shieldAbsorb = Mathf.Min(remainingDamage, player[playerIndex].Shield);
                        player[playerIndex].Shield -= shieldAbsorb;
                        remainingDamage -= shieldAbsorb;
                    }

                    // Apply leftover damage to health
                    if (remainingDamage > 0)
                    {
                        player[playerIndex].Health -= remainingDamage;
                    }

                    // Clamp health and handle death
                    if (player[playerIndex].Health <= 0)
                    {
                        player[playerIndex].Health = 0;
                        player[playerIndex].playerDead = true;

                        Debug.Log($"Player {playerIndex} died!");

                        if (playerIndex == 0 && !playerXDead)
                        {
                            player[1].Victories++;
                            playerXDead = true;
                        }
                        else if (playerIndex == 1 && !playerXDead)
                        {
                            player[0].Victories++;
                            playerXDead = true;
                        }
                    }

                    break;
                }
            case "ConsumeShield":                           //Negative value removes shield, positive value adds shield
                player[playerIndex].Shield += value;
                if (player[playerIndex].Shield < 0)
                {
                    player[playerIndex].Shield = 0;
                }
                break;

        }
    }

    public void ChangeGun(int PlayerIndex, string gunName)
    {
        player[PlayerIndex].CurrentGun = GunManager.Instance.GetGunData(gunName);   
    }

    public void ResetPlayerStats()                      //Reset every player stats, Exclude reseting TotalScore
    {
        for (int i = 0; i < player.Length; i++)
        {
            player[i].Score = 0;
            player[i].Health = 100;
            player[i].Shield = 100;
            //            player[i].CurrentGun = 0;
            player[i].CurrentGun = GunManager.Instance.GetGunData(defaultGun);   //Set players gun to default 
            GunManager.Instance.GetGunData(player[i].CurrentGun.GunName);       //Get default gunData
            player[i].playerDead = false;
        }

    }

    public string WhichPlayerIsDead()
    {
        //Determine which player is dead
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].playerDead)
            {
                return player[i].name;                
            }            
        }

        return null;
    }

    
}
/*
 *             case "TakeDamage":                              //Negative value removes Health, positive adds health

                //Compare if player has shield, decrease shield while shield >= 0 and then decrease health!
                if (player[playerIndex].Shield >  0)
                {
                    AffectPlayer(playerIndex, "ConsumeShield", value - player[playerIndex].Shield);
                }

                player[playerIndex].Health += value;        
                if (player[playerIndex].Health <= 0)
                {
                    Debug.Log($"Player 2 health {player[1].Health}, player 2 {player[1].Victories}");
                    Debug.Log($"Player 1 health {player[0].Health}, player 1 {player[0].Victories}");
                    player[playerIndex].Health = 0;
                    player[playerIndex].playerDead = true;
                    
                    if (playerIndex == 0 && !playerXDead)
                    {
                        player[1].Victories++;
                        Debug.Log($"player {player[1].Victories}");
                        playerXDead = true;
                    }
                    else if (playerIndex == 1 && !playerXDead)
                    {
                        player[0].Victories++;
                        Debug.Log($"player {player[0].Victories}");
                        playerXDead = true;
                    }
                }
                break;
*/