using Unity.VisualScripting;
using UnityEngine;

public class AdvancedGun : Gun
{
    protected override void Awake()
    {
        base.Awake();
        gunName = "AdvancedGun";
        fireRate = 2.0f;
        ammoCount = 3f;
        ammonition = 2.0f;
        speed = 7.0f;
        destroyTime = 3.0f;
    }
}

/**************************************************************************
 *                     Old code, partly not working
 *                     Use as reference if needed
 **************************************************************************                     
 /*
    [DoNotSerialize]
    new public string gunName { get; protected set; } = "AdvancedGun";
    new public float fireRate { get; protected set; } = 2.0f;
    new public float ammoCount { get; protected set; } = 10.0f;
    new public float ammonition { get; protected set; } = 2.0f;
    new public float speed { get; protected set; } = 7.0f;
    new public float destroyTime { get; protected set; } = 3.0f;


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
    */