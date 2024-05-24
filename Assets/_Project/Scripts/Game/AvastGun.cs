using System.Collections.Generic;
using UnityEngine;

public class AvastGun : Gun
{
    [SerializeField] private Material bulletMaterial;

    public override void Fire() 
    {
        GameObject bullet = hitscanBulletPool.GetChild(0).gameObject;

        bullet.SetActive(true);
        bullet.GetComponent<HitscanBullet>().Init(bulletMaterial,
                                                  activeBulletPool,
                                                  gunPoint.position,
                                                  target.position,
                                                  bulletDamage,
                                                  new List<Collider2D>() { circleCollider });
    }

    protected override void Rotate()
    {
        rotateAnchor.rotation = Quaternion.Slerp(rotateAnchor.rotation, Quaternion.AngleAxis(Vector2.SignedAngle(Vector3.up, target.position - rotateAnchor.position), Vector3.forward), 1f / rotationSpeed * Time.deltaTime);
    }
}