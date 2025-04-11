using UnityEngine;

public class BulletSpawnStopDetection2 : MonoBehaviour
{
    [SerializeField] private Player2Movement player2Movement;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelElement"))
        {
            player2Movement.allowShooting = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player2Movement.allowShooting = true;
    }
}
