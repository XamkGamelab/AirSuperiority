using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Player variables
    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private float rotationSpeed = 90;

    // Weapon variables
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject normalBullet;

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

        if (shootAction.IsPressed())
        {  
            PlayerShoot();
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
//        Debug.Log($"Player shot with: {}");
        

    }

    private void OnTriggerEnter(Collider other)
    {

        Gun gun = other.GetComponent<Gun>();
        if (gun != null)
        {
            EquipGun(gun.GetGunData());
            Destroy(other.gameObject);      //Remove gun
        }

    }

    private void EquipGun(GunData newGun)
    {
        

    }
}
