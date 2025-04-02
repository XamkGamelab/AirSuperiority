using UnityEngine;


public class GunData
{
    public string GunName;
    public float FireRate;                          //Variable could be for example tide to deltaTime
    public float AmmoCount;
    public string Ammonition;                        //Defines what kind of a projectile gun shoots
    public float Speed;                             //Bullet flying speed
    public float DestroyTime;                       //Time before bullet is destroyed
    public float Damage;                            //Affected damage to player
    public Sprite gunSprite;

    //Constructor
    public GunData(string gunName, float fireRate, float ammoCount, string ammonition, float speed, float destroyTime, float damage, Sprite gunSprite)
    {
        GunName = gunName;
        FireRate = fireRate;
        AmmoCount = ammoCount;
        Ammonition = ammonition;
        Speed = speed;
        DestroyTime = destroyTime;
        Damage = damage;
        this.gunSprite = gunSprite;
    }

    public GunData(GunData other)
    {
        if (other == null)
            return;
        GunName = other.GunName;
        FireRate = other.FireRate;
        AmmoCount = other.AmmoCount;
        Ammonition = other.Ammonition;
        Speed = other.Speed;
        DestroyTime = other.DestroyTime;
        Damage = other.Damage;
        this.gunSprite = other.gunSprite;

    }


}
