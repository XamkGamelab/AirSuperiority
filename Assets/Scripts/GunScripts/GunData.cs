using UnityEngine;


public class GunData
{
    public string GunName;
    public float FireRate;                          //Variable could be for example tide to deltaTime
    public float AmmoCount;
    public float Ammonition;                        //Defines what kind of a projectile gun shoots
    public float Speed;                            //Bullet flying speed
    public float DestroyTime;                      //Time before bullet is destroyed

    //Constructor
    public GunData(string gunName, float fireRate, float ammoCount, float ammonition, float speed, float destroyTime)
    {
        GunName = gunName;
        FireRate = fireRate;
        AmmoCount = ammoCount;
        Ammonition = ammonition;
        Speed = speed;
        DestroyTime = destroyTime;
    }


}
