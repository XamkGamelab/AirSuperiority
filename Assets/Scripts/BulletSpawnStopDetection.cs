using UnityEngine;

public class BulletSpawnStopDetection : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelElement"))
        {
            playerMovement.allowShooting = false;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerMovement.allowShooting = true;
    }
}
