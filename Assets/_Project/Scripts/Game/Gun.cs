using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("User Settings")]
    [SerializeField] private float aimRadius = 2f;
    [SerializeField][Dropdown("aimingModes")] private string aimingMode;
    [SerializeField][Dropdown("shootModes")] private string shootMode;
    [Space]
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletSpeed;
    [Space]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float shootingFOV = 20f;

    [Header("Dev Settings")]
    [SerializeField] private bool aim;
    [SerializeField] private Transform target;
    [SerializeField] private Transform body;
    [Space]
    [SerializeField] private Transform bulletPool;
    [SerializeField] private Transform activeBulletPool;

    private Timer timer_Shoot;
    private List<Transform> possibleTargets = new();
    private CircleCollider2D circleCollider;

    private Transform gunPoint;
    private SpriteRenderer gunSprite;

    private List<string> aimingModes = new List<string>() { "Closest", "First in group", "Last in group", "None" };
    private List<string> shootModes = new List<string>() { "Physical Bullets", "Hitscan Shots", "None" };

    private void Awake()
    {
        timer_Shoot = new Timer(shootCooldown, () => { Shoot(); }, false);
        circleCollider = GetComponent<CircleCollider2D>();
        gunPoint = body.transform.GetChild(0);
        gunSprite = body.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        circleCollider.radius = aimRadius;
        timer_Shoot.time = shootCooldown;

        Aim();

        UpdateTimers();
    }

    private void Aim() 
    {
        if (!target) return;

        if (aim) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(Vector2.SignedAngle(Vector3.up, target.position - transform.position), Vector3.forward), rotationSpeed * Time.deltaTime);

        timer_Shoot.playing = IsInShootFOV(target);
    }

    private void Shoot()
    {
        switch (shootMode) 
        {
            case "Physical Bullets":
                ShotBullet();
                break;
            case "Hitscan Shots":
                ShotHitscan();
                break;
            case "None":
                break;
        }
    }

    private void ShotBullet() 
    {
        GameObject bullet = bulletPool.GetChild(0).gameObject;
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        HealthImpacter2D bulletHI = bullet.GetComponent<HealthImpacter2D>();

        bullet.SetActive(true);
        bullet.transform.SetParent(activeBulletPool);
        bullet.transform.position = gunPoint.position;
        bulletRB.AddForce(gunPoint.up * bulletSpeed, ForceMode2D.Impulse);
        bulletHI.delta = -bulletDamage;
        bulletHI.dontHits.Add(circleCollider);
        bulletHI.onDisable.AddListener(() =>
        {
            bullet.transform.SetParent(bulletPool);
            bullet.transform.position = bulletPool.position;
            bulletRB.velocity = Vector2.zero;
            bulletHI.dontHits.Remove(circleCollider);
            bulletHI.onDisable.RemoveAllListeners();
        });
    }

    private void ShotHitscan() 
    {

    }

    private bool IsInShootFOV(Transform t) 
    {
        return Vector2.Angle(gunPoint.up, t.position - transform.position) <= shootingFOV;
    }

    private void UpdateTarget() 
    {
        if (possibleTargets.Count <= 0) 
        {
            target = null;
            return;
        }

        target = aimingMode switch
        {
            "Closest" => FindClosestTarget(),
            "First in group" => possibleTargets[^1],
            "Last in group" => possibleTargets[0],
            "None" => null,
            _ => null,
        };
    }

    public void Replace(GunData data) 
    {
        aimRadius = data.aimRadius;
        aimingMode = data.aimingMode;
        shootMode = data.shootMode;

        shootCooldown = data.shootCooldown;
        bulletDamage = data.bulletDamage;
        bulletSpeed = data.bulletSpeed;

        rotationSpeed = data.rotationSpeed;
        shootingFOV = data.shootingFOV;

        gunSprite.sprite = data.body.GetComponent<SpriteRenderer>().sprite;
        gunPoint.localPosition = data.body.transform.GetChild(0).localPosition;

        timer_Shoot.Reset();
    }

    private Transform FindClosestTarget() 
    {
        Transform closest = possibleTargets[0];

        for (int i = 1; i < possibleTargets.Count; i++) 
        {
            if ((possibleTargets[i].position - transform.position).sqrMagnitude <= (closest.position - transform.position).sqrMagnitude)
                closest = possibleTargets[i];
        }

        return closest;
    }

    private void UpdateTimers() 
    {
        timer_Shoot.Update();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(shootingFOV, Vector3.forward) * Vector2.up * aimRadius);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-shootingFOV, Vector3.forward) * Vector2.up * aimRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        { 
            possibleTargets.Add(collision.transform);
            UpdateTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            possibleTargets.Remove(collision.transform);
            UpdateTarget();
        }
    }
}
