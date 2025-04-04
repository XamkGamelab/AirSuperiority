using UnityEngine;

public class AdvancedGunBulletScript : MonoBehaviour
{
    private float speed;
    private float destroyTime;
    private int life = 2;
    private Vector2 velocity;

    private void Awake()
    {
        speed = GunManager.Instance.GetGunData("BasicGun").Speed;
        destroyTime = GunManager.Instance.GetGunData("BasicGun").DestroyTime;
    }

    void Start()
    {
        // initial velocity in the direction the bullet is facing
        velocity = transform.up * speed;

        // Destroy bullets 
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Bullet flies forward
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    // Detect if the bullet hit a player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        life--;
        if (life <= 0)
        {
            Destroy(gameObject);
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelElement"))
        {
            // Reflect the bullet's velocity based on the collision normal
            velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);

            // Calculate the new angle based on the reflected velocity
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // Rotate the bullet to match the reflected velocity direction
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);  // Adjusting by 90 degrees
        }
    }
}
