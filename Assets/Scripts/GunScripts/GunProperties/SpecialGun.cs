using Unity.VisualScripting;
using UnityEngine;

public class SpecialGun : Gun
{
    [SerializeField] private Sprite GunSprite;
    protected override void Awake()
    {
        base.Awake();
        gunName = "SpecialGun";
        fireRate = 2.0f;
        ammoCount = 3f;
        ammonition = "Gun3Bullet";
        speed = 7.0f;
        destroyTime = 3.0f;
        damage = 30.0f;
        gunSprite = GunSprite;
    }
}
