using Unity.VisualScripting;
using UnityEngine;

public class SpecialGun : Gun
{
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
    }
}
//Possible problem on playerdeath, game wont continue to next level.