using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Movement : MonoBehaviour
{

    // Player Variables
    public float movementSpeed = 2;
    public float rotationSpeed = 90;

    // InputActions
    InputAction moveAction2;
    InputAction rotateLeftAction2;
    InputAction rotateRightAction2;

    void Start()
    {
        // Get InputSystem actions
        moveAction2 = InputSystem.actions.FindAction("Player2Move");
        rotateLeftAction2 = InputSystem.actions.FindAction("Player2RotateLeft");
        rotateRightAction2 = InputSystem.actions.FindAction("Player2RotateRight");
    }

    void Update()
    {
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
}
