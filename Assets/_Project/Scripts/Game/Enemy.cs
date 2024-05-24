using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    public int reward;
    public int damage;
    public float speed;
    public float health;
    [Space]
    public AudioClip s_damageTaken;
    public AudioClip s_died;

    public UnityEvent onDeath;

    protected List<Vector3> waypoints = new();

    protected Vector3 moveDirectionNormalized;

    protected HealthModule healthModule;
    protected Rigidbody2D rb;

    private void Awake()
    {
        healthModule = GetComponent<HealthModule>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(List<Vector3> newWaypoints)
    {
        waypoints.Clear();
        waypoints.AddRange(newWaypoints);
        transform.position = waypoints[0];
    }

    protected virtual void Start()
    {
        healthModule.SetMaxHealth(health);
        healthModule.SetCurrentHealth(health);
        healthModule.onHealthChange.AddListener((float delta, float currentHealth) => {
            if (delta < 0 && currentHealth > 0) { AudioManager.instance.PlayOneShot("hit", transform.position); }
            else if (delta < 0) { AudioManager.instance.PlayOneShot("die", transform.position); Die(); }
        });

        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        UpdateDirection();
        Move();
        ClampSpeed();
    }

    private void UpdateDirection()
    {
        if ((waypoints[0] - transform.position).sqrMagnitude <= 0.01f)
            waypoints.Remove(waypoints[0]);
        if (waypoints.Count == 0)
        {
            Die(false);
            return;
        }

        moveDirectionNormalized = (waypoints[0] - transform.position).normalized;
    }

    protected abstract void Move();

    private void ClampSpeed() 
    {
        if (rb.velocity.sqrMagnitude > speed * speed) rb.velocity = rb.velocity.normalized * speed;
    }

    private void Die(bool giveReward = true)
    {
        onDeath.Invoke();
        if (giveReward) LevelManager.instance.MoneyCount += reward;
        else LevelManager.instance.HealthCount -= damage;
        gameObject.SetActive(false);
    }
}
