using System.Collections.Generic;
using UnityEngine;

public class DrWebGun : Gun 
{
    [SerializeField] private Sprite bulletSprite;

    public override void Fire() 
    {
        GameObject bullet = physicalBulletPool.GetChild(0).gameObject;

        bullet.SetActive(true);
        bullet.GetComponent<PhysicalBullet>().Init(bulletSprite,
                                                   activeBulletPool,
                                                   gunPoint.position,
                                                   gunPoint.up,
                                                   bulletSpeed,
                                                   bulletDamage,
                                                   new List<Collider2D>() { circleCollider });
    }

    protected override void Rotate()
    {
        rotateAnchor.rotation = Quaternion.Slerp(rotateAnchor.rotation, Quaternion.AngleAxis(Vector2.SignedAngle(Vector3.up, target.position - rotateAnchor.position), Vector3.forward), 1f / rotationSpeed * Time.deltaTime);
    }
}