using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
        protected string gunName;
        protected float fireRate;
        protected float ammoCount;
        protected float ammonition;
        protected float speed;
        protected float destroyTime;

    */
    [SerializeField] public string gunName = "";
    [SerializeField] public float fireRate = 1.0f;
    [SerializeField] public float ammoCount = 5.0f;
    [SerializeField] public float ammonition = 0.0f;
    [SerializeField] public float speed = 5.0f;
    [SerializeField] public float destroyTime = 3.0f;

    public string GunName => gunName;       //Expose as read-only property
    public float FireRate => fireRate;
    public float AmmoCount => ammoCount;
    public float Ammonition => ammonition;
    public float Speed => speed;
    public float DestroyTime => destroyTime;

    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(gunName))
        {
            gunName = gameObject.name;  // Default to prefab name if null
            Debug.Log($"GunName set to: {gunName}");
        }
        /***************
        //string.IsNullOrEmpty(gunName) ? gameObject.name : gunName;
        gunName = string.IsNullOrEmpty(gunName) ? gameObject.name : gunName; //"Default Gun";
        fireRate = 1.0f;
        ammoCount = 5.0f;
        ammonition = 0.0f;
        speed = 5.0f;
        destroyTime = 3f;
        ***************/
    }

    public GunData GetGunData()
    {
        return new GunData
        {
            GunName = this.GunName,
            FireRate = this.FireRate,
            AmmoCount = this.AmmoCount,
            Ammonition = this.Ammonition,
            Speed = this.Speed,
            DestroyTime = this.DestroyTime
        };
    }

}