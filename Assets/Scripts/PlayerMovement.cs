using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Player Variables
    public float movementSpeed = 2;
    public float rotationSpeed = 90;

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
            // Add cooldown between shots... 

            PlayerShoot();
        }
    }

    void MovePlayer()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
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
        Debug.Log($"Shoot Action is Called");

        // Check which weapon the player has...

        // Check player direction...

        // Instantiate bullet prefab...

    }

}
