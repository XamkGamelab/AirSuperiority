using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

/****************************************************
 *              Instructions
 **************************************************** 
 *
 *       //Example how to use CurrentGun data inside PlayerData
 *       Debug.Log($"Player shot with: {StatsManager.Instance.player[player].CurrentGun.GunName}");
 * 
 * 
 * 
 * 
 */

public class PlayerMovement : MonoBehaviour
{
    // Player variables
    private float acceleration = 2f;
    private float deceleration = 3f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float reverseSpeed = 1f;
    private Vector2 direction = Vector2.zero;
    private Vector2 moveValue = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    [SerializeField] private bool isMoving = false;

    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float rotMoveSpeed = 60f;
    private float rotDecel = 40f;

    [SerializeField] private int player = 0;
    [SerializeField] private int enemy = 1;
    [SerializeField] private bool kamikaze = false;

    // Weapon variables
    [SerializeField] private Transform bulletSpawnPoint;
    private float fireRate = 1f;

    private float time;

    private GameObject bulletInst;

    // InputActions
    InputAction moveAction;
    InputAction rotateLeftAction;
    InputAction rotateRightAction;
    InputAction shootAction;

    private void Start()
    {
        // Get InputSystem actions
        moveAction = InputSystem.actions.FindAction("Move");
        rotateLeftAction = InputSystem.actions.FindAction("RotateLeft");
        rotateRightAction = InputSystem.actions.FindAction("RotateRight");
        shootAction = InputSystem.actions.FindAction("Shoot");
        Debug.Log($"GameManager state isPlaying: {GameManager.Instance.isPlaying}");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPlaying == true) 
        { 
            // Count time for firerate
            time += Time.deltaTime;

            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            Debug.Log(moveValue);

            // Move Player forward/backward
            if (moveAction.IsPressed())
            {
                isMoving = true;
                direction = moveValue.normalized;
                if (moveValue.y < 0)
                {
                    velocity = Vector2.MoveTowards(velocity, Vector2.down, reverseSpeed * Time.deltaTime);
                }
                else if (moveValue.y >= 0)
                {
                    velocity += acceleration * Time.deltaTime * direction;

                    velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
                }
            } 
            else if (!moveAction.IsPressed()) 
            {
                isMoving = false;
                velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);
            }

            transform.Translate(velocity * Time.deltaTime);


            // Normal player rotation
            if (rotateLeftAction.IsPressed())
                RotatePlayerLeft();
            if (rotateRightAction.IsPressed())
                RotatePlayerRight();
            

            // Player shooting action
            if (shootAction.IsPressed())
            {
                if (StatsManager.Instance.player[player].CurrentGun.GunName != "BasicGun")
                {
                    if (time > fireRate / StatsManager.Instance.player[player].CurrentGun.FireRate) 
                    {
                        PlayerShoot();
                        time = 0;
                    }
                } else if (StatsManager.Instance.player[player].CurrentGun.GunName == "BasicGun")
                {
                    if (time > fireRate) 
                    {
                        PlayerShoot();
                        time = 0;
                    }
                }
            }
        }
    }

    void RotatePlayerLeft()
    {
        if (!isMoving)
        {
            rotationSpeed = 120f;
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        } 
        else if (isMoving)
        {
            if (rotationSpeed >= rotMoveSpeed)
            {
                rotationSpeed -= rotDecel * Time.deltaTime;
            }

            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        
    }

    void RotatePlayerRight()
    {
        if (!isMoving)
        {
            rotationSpeed = 120f;
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        else if (isMoving)
        {
            if (rotationSpeed >= rotMoveSpeed)
            {
                rotationSpeed -= rotDecel * Time.deltaTime;
            }

            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }

    void PlayerShoot()
    {
        Debug.Log($"Shoot Action is Called");

        if (StatsManager.Instance.player[player].CurrentGun.AmmoCount >= 1)
        {
            //Substract player ammoCount by 1 
            StatsManager.Instance.player[player].CurrentGun.AmmoCount -= 1;

            Debug.Log($"Player1 ammoCount: {StatsManager.Instance.player[0].CurrentGun.AmmoCount}, Player2 ammoCount: {StatsManager.Instance.player[1].CurrentGun.AmmoCount}");

            // Need to figure out which script calls the shoot() function. Guns can be stored in a list or array and can be called from there: gun[0].shoot(); etc. This the retrieves the bullet fired.

            // Instantiate bullet prefab...
            bulletInst = (GameObject)Instantiate(Resources.Load($"Prefabs/Bullets/{StatsManager.Instance.player[player].CurrentGun.Ammonition}"), bulletSpawnPoint.position, transform.rotation);

            //Example how to use CurrentGun data inside PlayerData
//            Debug.Log($"Player shot with: {StatsManager.Instance.player[player].CurrentGun.GunName}");
//            Debug.Log($"Player has {StatsManager.Instance.player[player].CurrentGun.AmmoCount} bullets left.");
        }
        else
        {
            Debug.Log($"Player {player} is out of ammo!");
        }
    }

    // Detect a gun pickup
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BasicGun"))
        {
            Debug.Log($"BASIC GUN PICKED UP");
            StatsManager.Instance.ChangeGun(player, "BasicGun");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("AdvancedGun"))
        {
            Debug.Log($"ADVANCED GUN PICKED UP");
            StatsManager.Instance.ChangeGun(player, "AdvancedGun");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("SpecialGun"))
        {
            Debug.Log($"Special GUN PICKED UP");
            StatsManager.Instance.ChangeGun(player, "SpecialGun");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Health"))
        {
            Debug.Log($"HEALTH PICKED UP");
            Destroy(collision.gameObject);
            for (int j = 0; j < 25; j++) 
            {
                
                if (StatsManager.Instance.player[player].Health <= 0)
                {
                    return;
                }
                else if (StatsManager.Instance.player[player].Health != 100)
                {
                    StatsManager.Instance.AffectPlayer(player, "TakeDamage", 1f);
                }
                else
                {
                    return;
                }
                
            } 
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            Debug.Log($"HEALTH PICKED UP");
            Destroy(collision.gameObject);
            for (int l = 0; l < 25; l++)
            {
                if (StatsManager.Instance.player[player].Shield != 100)
                {
                    StatsManager.Instance.AffectPlayer(player, "ConsumeShield", 1f);
                }
                else
                {
                    return;
                }

            }
        }



        /*Gun gun = other.GetComponent<Gun>();
        if (gun != null)
        {
            //            EquipGun(gun.GetGunData());                   //Possible to read data from instantiated gun script, prefer to use GunName and player index to call
            //                                                          StatsManager.Instance.ChangeGun(int PlayerIndex, string gunName). Data is on preloaded GunData array.
            string _name = gun.name;
            Debug.Log($"Trying to Equip gun named: {_name}");
            StatsManager.Instance.ChangeGun(player, _name);
//            EquipGun(_name, player);                      //Dont use this method
            Destroy(other.gameObject);      //Remove gun

        }
        */
    }

    // Detect bullet collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log($"!!!!! Player1 hit !!!!!");
            CalculateDamage();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            kamikaze = true;
            CalculateDamage();
        }

    }

    private void EquipGun(string newGun, int PlayerIndex)   //Do not use this method for changing gun, instead use StatsManager.Instance.ChangeGun(int PlayerIndex, string gunName)
    {
       StatsManager.Instance.EquipGun(newGun, PlayerIndex); //Cals method to change CurrentGun by gun name into PlayerData

    }

    /*
    private void PlayerNewGun()
    {
        StatsManager.Instance.ChangeGun(player, GET THE GUN NAME)
    }
    */

    // Calculate how much damage is taken and does damage affect shield or health
    private void CalculateDamage()
    {
        // Check which bullet hit the player for better damage calculation
        // Write method for getting current gun bullet damage
        // ^^ This could maybe be GunData ^^

        /*if (StatsManager.Instance.player[player].Shield == 0)
        {
            StatsManager.Instance.AffectPlayer(player, "TakeDamage", bulletDamage);
        } else if (StatsManager.Instance.player[player].Shield != 0)
        {
            StatsManager.Instance.AffectPlayer(player, "ConsumeShield", bulletDamage);
        }*/

        if (!kamikaze) { 
            float bulletDamage = StatsManager.Instance.player[enemy].CurrentGun.Damage;

        for (int i = 0; i < bulletDamage; i++)
        {

                if (StatsManager.Instance.player[player].Shield == 0)
                {
                    StatsManager.Instance.AffectPlayer(player, "TakeDamage", -1);
                }
                else if (StatsManager.Instance.player[player].Shield != 0)
                {
                    StatsManager.Instance.AffectPlayer(player, "ConsumeShield", -1);
                }
            }

            if (StatsManager.Instance.player[player].Health <= 0)
            {
                // Check if player is alive, if not alive -> destroy player, or hide player?
                StatsManager.Instance.AffectPlayer(enemy, "AddScore", 10);
                Destroy(gameObject);
            }
        }
        else if (kamikaze)
        {
            Destroy(gameObject);
            StatsManager.Instance.playerXDead = true;
        }

    }

}
