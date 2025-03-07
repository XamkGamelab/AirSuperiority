using UnityEngine;

public class BasicGun : Gun
{
    private void Start()
    {
        gunName = "BasicGun";
        fireRate = 1;                          //Variable could be for example tide to deltaTime
        ammoCount = 50;
        ammonition = 1;                        //Defines what kind of a projectile gun shoots
        speed = 5f;                            //Bullet flying speed
        destroyTime = 3f;                      //Time before bullet is destroyed        
    
        if (string.IsNullOrEmpty(gunName))
        {
            gunName = gameObject.name;
            Debug.Log($"GunName set in Start: {gunName}");
        }
    }

    /*
    protected override void Awake()
    {
        base.Awake();                              //Call base class Awake() if needed           
            gunName = "BasicGun";
            fireRate = 1;                          //Variable could be for example tide to deltaTime
            ammoCount = 5;
            ammonition = 1;                        //Defines what kind of a projectile gun shoots
            speed = 5f;                            //Bullet flying speed
            destroyTime = 3f;                      //Time before bullet is destroyed        
    }
    */
}
