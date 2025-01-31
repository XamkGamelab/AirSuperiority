using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Player1 Variables
    public float movementSpeed = 2;
    public float leftRotationSpeed = 60;
    public float rightRotationSpeed = -60;

    // InputActions
    InputAction moveAction;
    InputAction rotateLeftAction;
    InputAction rotateRightAction;

    private void Start()
    {
        // Get InputSystem actions
        moveAction = InputSystem.actions.FindAction("Move");
        rotateLeftAction = InputSystem.actions.FindAction("RotateLeft");
        rotateRightAction = InputSystem.actions.FindAction("RotateRight");
    }

    // Update is called once per frame
    void Update()
    {
        // Move Player1 forward/backward
        if (moveAction.IsPressed())
        {
            MovePlayer();
        }

        // Rotate Player1 left ** KORJAA TÄMÄ **
        if (rotateLeftAction.IsPressed())
        {
            RotatePlayerLeft();
        } 
        else if (rotateLeftAction.IsPressed() && Input.GetKey(KeyCode.S))
        {
            RotatePlayerBackwardLeft();
            Debug.Log("Taaksepäinvasemmalle");
        }

        // Rotate Player1 right ** KORJAA TÄMÄ **
        if (rotateRightAction.IsPressed())
        {
            RotatePlayerRight();
        }
        else if (rotateRightAction.IsPressed() && Input.GetKey(KeyCode.S))
        {
            RotatePlayerBackwardRight();
            Debug.Log("Taaksepäinoikealle");
        }
    }

    void MovePlayer()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        transform.Translate(new Vector2(0, moveValue.y) * movementSpeed * Time.deltaTime);
    }

    void RotatePlayerLeft()
    {
        transform.Rotate(0, 0, leftRotationSpeed * Time.deltaTime);
    }

    void RotatePlayerBackwardLeft()
    {
        transform.Rotate(0, 0, leftRotationSpeed * Time.deltaTime);
    }

    void RotatePlayerRight()
    {
        transform.Rotate(0, 0, rightRotationSpeed * Time.deltaTime);
    }

    void RotatePlayerBackwardRight()
    {
        transform.Rotate(0, 0, rightRotationSpeed * Time.deltaTime);
    }
}
