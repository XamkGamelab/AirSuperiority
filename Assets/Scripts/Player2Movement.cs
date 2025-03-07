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
    [SerializeField] private GameObject normalBullet;
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

            if (shootAction2.IsPressed() && time > fireRate)
            {
                PlayerShoot();
                time = 0;
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

        // Check which weapon the player has...

        // Instantiate bullet prefab...
        bulletInst = Instantiate(normalBullet, bulletSpawnPoint.position, transform.rotation);
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
        float bulletDamage = -50;

        if (StatsManager.Instance.player[player].Shield == 0)
        {
            StatsManager.Instance.AffectPlayer(player, "TakeDamage", bulletDamage);
        }
        else if (StatsManager.Instance.player[player].Shield != 0)
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
