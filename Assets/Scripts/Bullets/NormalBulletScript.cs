using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float destroyTime = 3f;

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

        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelElement"))
        {
            Destroy(gameObject);
        }
    }
}
