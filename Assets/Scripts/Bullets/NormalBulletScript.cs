using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    private float speed;
    private float destroyTime;

    private void Awake()
    {
        speed = GunManager.Instance.GetGunData("BasicGun").Speed;
        destroyTime = GunManager.Instance.GetGunData("BasicGun").DestroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Bullet flies forward
        transform.position += speed * Time.deltaTime * transform.up;

        // Destroy bullets 
        Destroy(gameObject, destroyTime);
    }

    // Detect if the bullet hit a player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelElement"))
        {
            Destroy(gameObject);
        }
    }
}
