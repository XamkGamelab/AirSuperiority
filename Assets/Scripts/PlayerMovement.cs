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
    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private float rotationSpeed = 90;
    [SerializeField] private int player = 0;
    [SerializeField] private int enemy = 1;

    // Weapon variables
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject normalBullet;
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

            // Move Player forward/backward
            if (moveAction.IsPressed())
            {
                MovePlayer();
            }

            // Player rotation inverted when moving backwards
            if (Input.GetKey(KeyCode.S))
            {
                // Inverted player rotation
                if (rotateLeftAction.IsPressed())
                    transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
                if (rotateRightAction.IsPressed())
                    transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
            else 
            {
                // Normal player rotation
                if (rotateLeftAction.IsPressed())
                    RotatePlayerLeft();
                if (rotateRightAction.IsPressed())
                    RotatePlayerRight();
            }

            if (shootAction.IsPressed() && time > fireRate)
            {
                PlayerShoot();
                time = 0;
            }
        }
    }

    void MovePlayer()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        transform.Translate(movementSpeed * Time.deltaTime * new Vector2(0, moveValue.y));
    }

    void RotatePlayerLeft()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    void RotatePlayerRight()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    void PlayerShoot()
    {
        Debug.Log($"Shoot Action is Called");

        // Check which weapon the player has...

        // Need to figure out which script calls the shoot() function. Guns can be stored in a list or array and can be called from there: gun[0].shoot(); etc. This the retrieves the bullet fired.
        // Instantiate bullet prefab...
        bulletInst = Instantiate(normalBullet, bulletSpawnPoint.position, transform.rotation);
        //Example how to use CurrentGun data inside PlayerData
        Debug.Log($"Player shot with: {StatsManager.Instance.player[player].CurrentGun.GunName}");
        Debug.Log($"Player has {StatsManager.Instance.player[player].CurrentGun.AmmoCount} bullets left.");
        Debug.Log($"Players GunSprite is {StatsManager.Instance.player[player].CurrentGun.gunSprite} .");
        Debug.Log($"Players GunSprite is {StatsManager.Instance.player[player].CurrentGun.Ammonition} .");
    }

    private void OnTriggerEnter(Collider other)
    {

        Gun gun = other.GetComponent<Gun>();
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

    }

    // Detect bullet collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log($"!!!!! Player1 hit !!!!!");
            CalculateDamage();
        }
    }

    private void EquipGun(string newGun, int PlayerIndex)   //Do not use this method for changing gun, instead use StatsManager.Instance.ChangeGun(int PlayerIndex, string gunName)
    {
       StatsManager.Instance.EquipGun(newGun, PlayerIndex); //Cals method to change CurrentGun by gun name into PlayerData

    }

    // Calculate how much damage is taken and does damage affect shield or health
    private void CalculateDamage()
    {   
        // Check which bullet hit the player for better damage calculation
        // Write method for getting current gun bullet damage
        // ^^ This could maybe be GunData ^^
        float bulletDamage = -50f;

        if (StatsManager.Instance.player[player].Shield == 0)
        {
            StatsManager.Instance.AffectPlayer(player, "TakeDamage", bulletDamage);
        } else if (StatsManager.Instance.player[player].Shield != 0)
        {
            StatsManager.Instance.AffectPlayer(player, "ConsumeShield", bulletDamage);
        }

        // Check if player is alive, if not alive -> destroy player, or hide player?
        if (StatsManager.Instance.player[player].Health == 0)
        {
            StatsManager.Instance.AffectPlayer(enemy, "AddScore", 10);
            Destroy(gameObject);
        }
    }

}
