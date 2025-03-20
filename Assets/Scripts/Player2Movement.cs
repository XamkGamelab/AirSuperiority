using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Movement : MonoBehaviour
{
    // Player Variables
    public float movementSpeed = 2;
    public float rotationSpeed = 90;
    [SerializeField] private int player = 1;
    [SerializeField] private int enemy = 0;

    // Weapon variables
    [SerializeField] private Transform bulletSpawnPoint;
    private float fireRate = 1f;
    public NormalBullet normalBulletScript;

    private float time;

    private GameObject bulletInst;

    // InputActions
    InputAction moveAction2;
    InputAction rotateLeftAction2;
    InputAction rotateRightAction2;
    InputAction shootAction2;

    void Start()
    {
        // Get InputSystem actions
        moveAction2 = InputSystem.actions.FindAction("Player2Move");
        rotateLeftAction2 = InputSystem.actions.FindAction("Player2RotateLeft");
        rotateRightAction2 = InputSystem.actions.FindAction("Player2RotateRight");
        shootAction2 = InputSystem.actions.FindAction("Player2Shoot");
        Debug.Log($"GameManager state isPlaying: {GameManager.Instance.isPlaying}");
        normalBulletScript.GetComponent<NormalBullet>();
    }

    void Update()
    {
        if (GameManager.Instance.isPlaying == true)
        {
            // Count time for firerate
            time += Time.deltaTime;

            // Move Player forward/backward
            if (moveAction2.IsPressed())
            {
                MovePlayer();
            }

            // Player rotation inverted when moving backwards
            if (Input.GetKey(KeyCode.DownArrow))
            {
                // Inverted player rotation
                if (rotateLeftAction2.IsPressed())
                    transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
                if (rotateRightAction2.IsPressed())
                    transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // Normal player rotation
                if (rotateLeftAction2.IsPressed())
                    RotatePlayerLeft();
                if (rotateRightAction2.IsPressed())
                    RotatePlayerRight();
            }

            // Player shooting action
            if (shootAction2.IsPressed())
            {
                if (StatsManager.Instance.player[player].CurrentGun.GunName != "BasicGun")
                {
                    if (time > fireRate / StatsManager.Instance.player[player].CurrentGun.FireRate)
                    {
                        PlayerShoot();
                        time = 0;
                    }
                }
                else if (StatsManager.Instance.player[player].CurrentGun.GunName == "BasicGun")
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

    void MovePlayer()
    {
        Vector2 moveValue = moveAction2.ReadValue<Vector2>();
        transform.Translate(new Vector2(0, moveValue.y) * movementSpeed * Time.deltaTime);
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
        Debug.Log($"Player2Shoot Action is Called");

        // Tell the bullet script which player shot. The current implementation might need to be changed if the player is also turned into a prefab.
        normalBulletScript.whoShot = player;

        // Instantiate bullet prefab...
        bulletInst = (GameObject)Instantiate(Resources.Load($"Prefabs/Bullets/{StatsManager.Instance.player[player].CurrentGun.Ammonition}"), bulletSpawnPoint.position, transform.rotation);
    }

    // Detect bullet collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log($"!!!!! Player2 hit !!!!!");
            CalculateDamage();
        }
    }

    private void CalculateDamage()
    {
        // Check which bullet hit the player for better damage calculation
        // Write method for getting current gun bullet damage
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

        // Check if player is alive, if not alive -> destroy player, or hide player?
        if (StatsManager.Instance.player[player].Health == 0)
        {
            StatsManager.Instance.AffectPlayer(enemy, "AddScore", 10);
            Destroy(gameObject);
        }
    }
}
