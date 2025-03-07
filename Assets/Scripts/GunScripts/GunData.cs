using UnityEngine;

[System.Serializable]
public class GunData
{
    public string GunName;
    public float FireRate;                          //Variable could be for example tide to deltaTime
    public float AmmoCount;
    public float Ammonition;                        //Defines what kind of a projectile gun shoots
    public float Speed;                            //Bullet flying speed
    public float DestroyTime;                      //Time before bullet is destroyed

}
