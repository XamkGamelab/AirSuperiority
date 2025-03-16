using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    private float speed = 5f;
    private float destroyTime = 3f;

    private void Awake()
    {
        speed = StatsManager.Instance.player[0].CurrentGun.Speed;
        destroyTime = StatsManager.Instance.player[0].CurrentGun.DestroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Bullet flies forward
        transform.position += transform.up * Time.deltaTime * speed;

        // Destroy bullets 
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
