using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class SpecialGun : Gun
{
    private StudioEventEmitter emitter;
    
    [SerializeField] private Sprite GunSprite;
    protected override void Awake()
    {
        base.Awake();
        gunName = "SpecialGun";
        fireRate = 1.0f;
        ammoCount = 3f;
        ammonition = "Gun3Bullet";
        speed = 5.0f;
        destroyTime = 3.0f;
        damage = 75.0f;
        gunSprite = GunSprite;
        gunID = 2;
    }

    private void Start()
    {
        emitter = AudioController.Instance.InitializeEventEmitter(FMODEvents.Instance.pickUpIdle, this.gameObject);
//        emitter.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            emitter.Stop();
            Destroy(this.gameObject);
    }
}
//Possible problem on playerdeath, game wont continue to next level.