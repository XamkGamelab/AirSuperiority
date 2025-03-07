using UnityEngine;

public class AdvancedGun : Gun
{

    protected override void Awake()
    {
        base.Awake();                          //Call base class Awake() if needed    
        gunName = "AdvancedGun22222";
        fireRate = 2.0f;                          //Variable could be for example tide to deltaTime
        ammoCount = 10.0f;
        ammonition = 2.0f;                        //Defines what kind of a projectile gun shoots
        speed = 7.0f;                            //Bullet flying speed
        destroyTime = 3.0f;                      //Time before bullet is destroyed

    }
}

