using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [Header("User Settings")]
    [SerializeField] protected float aimRadius = 2f;
    [SerializeField][Dropdown("aimingModes")] protected string aimingMode;
    [Space]
    [SerializeField] protected float shootCooldown = 1f;
    [SerializeField] protected float bulletDamage;
    [SerializeField] protected float bulletSpeed;
    [Space]
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float shootingFOV = 20f;

    [Header("Dev Settings")]
    [SerializeField] protected bool aim;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform body;
    [Space]
    [SerializeField] protected Transform physicalBulletPool;
    [SerializeField] protected Transform hitscanBulletPool;
    [SerializeField] protected Transform activeBulletPool;
    public GunData gunData;

    protected Timer timer_Shoot;
    protected List<Transform> possibleTargets = new();
    protected CircleCollider2D circleCollider;

    protected Transform gunPoint;
    protected SpriteRenderer gunSprite;
    protected Animator animator;
    protected Transform rotateAnchor;

    private List<string> aimingModes = new List<string>() { "Closest", "First in group", "Last in group", "None" };

    private void Awake()
    {
        timer_Shoot = new Timer(shootCooldown, () => { Shoot(); }, false);
        circleCollider = GetComponent<CircleCollider2D>();
        gunPoint = body.transform.GetChild(0);
        gunSprite = body.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rotateAnchor = transform.parent;
    }

    private void Start()
    {
        LoadData(gunData);
    }

    private void Update()
    {
        circleCollider.radius = aimRadius;
        timer_Shoot.time = shootCooldown;
        animator.SetFloat("speed", 1f / shootCooldown * 3);
        animator.SetBool("shooting", target != null && IsInShootFOV(target));

        Aim();

        UpdateTimers();
    }

    private void Aim() 
    {
        if (!target) return;

        if (aim) Rotate();

        timer_Shoot.playing = IsInShootFOV(target);
    }

    protected abstract void Rotate();

    private void Shoot() 
    {
        animator.Play("Shoot", 0, 0);
    }

    public abstract void Fire();

    private bool IsInShootFOV(Transform t) 
    {
        return t.gameObject.activeSelf && Vector2.Angle(gunPoint.up, t.position - transform.position) <= shootingFOV && (t.position - transform.position).sqrMagnitude <= aimRadius * aimRadius;
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

    private void LoadData(GunData data) 
    {
        aimRadius = data.aimRadius;
        
        shootCooldown = data.shootCooldown;
        bulletDamage = data.bulletDamage;
        bulletSpeed = data.bulletSpeed;
        
        aim = data.aim;
        if (!aim) rotateAnchor.rotation = Quaternion.identity;
        rotationSpeed = data.rotationSpeed;
        shootingFOV = data.shootingFOV;
        
        gunSprite.sprite = data.gunSprite;
        gunPoint.localPosition = data.gunPointLocalPosition;
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

    private void OnEnable()
    {
        timer_Shoot.Reset();
    }
}
