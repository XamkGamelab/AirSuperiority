using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    private float speed;
    private float destroyTime;
    public int whoShot;

    private void Awake()
    {
        speed = StatsManager.Instance.player[whoShot].CurrentGun.Speed;
        destroyTime = StatsManager.Instance.player[whoShot].CurrentGun.DestroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Bullet flies forward
        transform.position += transform.up * Time.deltaTime * speed;

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
    }
}
