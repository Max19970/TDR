using System.Collections.Generic;
using UnityEngine;

public class DefenderGun : Gun
{
    [SerializeField] private Sprite bulletSprite;

    public override void Fire()
    {
        GameObject bullet = physicalBulletPool.GetChild(0).gameObject;

        bullet.SetActive(true);
        bullet.GetComponent<PhysicalWave>().Init(bulletSprite,
                                                   activeBulletPool,
                                                   gunPoint.position,
                                                   gunPoint.up,
                                                   bulletSpeed,
                                                   bulletDamage,
                                                   new List<Collider2D>() { circleCollider });
    }

    protected override void Rotate()
    {
        rotateAnchor.rotation = Quaternion.identity;
    }
}