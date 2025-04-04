using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Movement : MonoBehaviour
{
    // Player Variables
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

    [SerializeField] private int player = 1;
    [SerializeField] private int enemy = 0;
    [SerializeField] private bool kamikaze = false;

    // Weapon variables
    [SerializeField] private Transform bulletSpawnPoint;
    private float fireRate = 1f;

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
    }

    void Update()
    {
        if (GameManager.Instance.isPlaying == true)
        {
            // Count time for firerate
            time += Time.deltaTime;

            Vector2 moveValue = moveAction2.ReadValue<Vector2>();
            Debug.Log(moveValue);

            // Move Player forward/backward
            if (moveAction2.IsPressed())
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
            else if (!moveAction2.IsPressed())
            {
                isMoving = false;
                velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);
            }

            transform.Translate(velocity * Time.deltaTime);


            // Normal player rotation
            if (rotateLeftAction2.IsPressed())
                RotatePlayerLeft();
            if (rotateRightAction2.IsPressed())
                RotatePlayerRight();


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
        Debug.Log($"Player2Shoot Action is Called");

        // Instantiate bullet prefab...
        bulletInst = (GameObject)Instantiate(Resources.Load($"Prefabs/Bullets/{StatsManager.Instance.player[player].CurrentGun.Ammonition}"), bulletSpawnPoint.position, transform.rotation);
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
    }

    // Detect bullet collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log($"!!!!! Player2 hit !!!!!");
            CalculateDamage();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            kamikaze = true;
            CalculateDamage();
        }
    }

    private void CalculateDamage()
    {
        // Check which bullet hit the player for better damage calculation
        // Write method for getting current gun bullet damage
        if (!kamikaze)
        {
            float bulletDamage = StatsManager.Instance.player[enemy].CurrentGun.Damage;

            for (int i = 0; i < bulletDamage; i++)
            {
                if (StatsManager.Instance.player[player].Health <= 0)
                {
                    // Check if player is alive, if not alive -> destroy player, or hide player?
                    StatsManager.Instance.AffectPlayer(enemy, "AddScore", 10);
                    Destroy(gameObject);
                    StatsManager.Instance.playerXDead = true;
                    return;
                }
                else if (StatsManager.Instance.player[player].Shield == 0)
                {
                    StatsManager.Instance.AffectPlayer(player, "TakeDamage", -1);
                }
                else if (StatsManager.Instance.player[player].Shield != 0)
                {
                    StatsManager.Instance.AffectPlayer(player, "ConsumeShield", -1);
                }
            }

        }
        else if (kamikaze)
        {
            Destroy(gameObject);
            StatsManager.Instance.playerXDead = true;
        }
    }
}
