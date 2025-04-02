using UnityEngine;

public class SpecialGunBulletScript : MonoBehaviour
{
    private float speed;
    private float destroyTime;
    private int bounceCount = 0;
    private bool bounced;

    private void Awake()
    {
        speed = GunManager.Instance.GetGunData("SpecialGun").Speed;
        destroyTime = GunManager.Instance.GetGunData("SpecialGun").DestroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(bounced)
        {
            // Bullet bounces based on angle
            transform.position += speed * Time.deltaTime * -transform.up;
        } 
        else if (!bounced)
        {
            // Bullet flies forward
            transform.position += speed * Time.deltaTime * transform.up;
        }

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
            if (bounceCount >= 2)
            {
                Destroy(gameObject);
                bounced = false;
                bounceCount = 0; 
            } 
            else
            {
                bounced = true;
                bounceCount++;
            }

        }
    }
}
