using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BasicGun : Gun
{
    [SerializeField] private Sprite GunSprite;
    protected override void Awake()
    {
        base.Awake();
        gunName = "BasicGun";
        fireRate = 1;
        ammoCount = 4;
        ammonition = "Gun1Bullet";
        speed = 10f;
        destroyTime = 3f;
        damage = 20.0f;
        gunSprite = GunSprite;
}
}

/**************************************************************************
 *                     Old code, partly not working
 *                     Use as reference if needed
 **************************************************************************                     
 *
    [DoNotSerialize]
    new public string gunName { get; protected set; } = "BasicGun";
    new public float fireRate { get; protected set; } = 1;
    new public float ammoCount { get; protected set; } = 5;
    new public float ammonition { get; protected set; } = 1;
    new public float speed { get; protected set; } = 5f;
    new public float destroyTime { get; protected set; } = 3f;

    //    private void Start()
    //    {

    /*
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
    */
//    }        
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

