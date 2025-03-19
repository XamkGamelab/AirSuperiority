using UnityEngine;

public class Gun : MonoBehaviour
{
    protected string gunName = "DefaultGun";
    protected float fireRate = 1.0f;
    protected float ammoCount = 5.0f;
    protected float ammonition = 0.0f;
    protected float speed = 5.0f;
    protected float destroyTime = 3.0f;
    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(gunName))
        {
            gunName = gameObject.name;  // Default to prefab name if null
            Debug.Log($"GunName set to: {gunName}");
        }
    }

    //Constructor call
    public virtual GunData GetGunData()
    {
        return new GunData(gunName, fireRate, ammoCount, ammonition, speed, destroyTime);        
    }

}

/**************************************************************************
 *                     Old code, partly not working
 *                     Use as reference if needed
 **************************************************************************                     
 * 
 *     /*
    public string gunName { get; protected set; } 
    public float fireRate { get; protected set; }
    public float ammoCount { get; protected set; }
    public float ammonition { get; protected set; }
    public float speed { get; protected set; }
    public float destroyTime { get; protected set; }
    */




/*
[SerializeField] public string gunName = "";
[SerializeField] public float fireRate = 1.0f;
[SerializeField] public float ammoCount = 5.0f;
[SerializeField] public float ammonition = 0.0f;
[SerializeField] public float speed = 5.0f;
[SerializeField] public float destroyTime = 3.0f;
*/
/*
public string GunName => gunName;       //Expose as read-only property
public float FireRate => fireRate;
public float AmmoCount => ammoCount;
public float Ammonition => ammonition;
public float Speed => speed;
public float DestroyTime => destroyTime;

*/

/***************
//string.IsNullOrEmpty(gunName) ? gameObject.name : gunName;
gunName = string.IsNullOrEmpty(gunName) ? gameObject.name : gunName; //"Default Gun";
fireRate = 1.0f;
ammoCount = 5.0f;
ammonition = 0.0f;
speed = 5.0f;
destroyTime = 3f;
***************/
