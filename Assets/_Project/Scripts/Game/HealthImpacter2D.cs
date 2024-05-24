using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class HealthImpacter2D : MonoBehaviour
{
    public bool working = false;

    [Header("Health Impact parameters")]
    public float delta;
    public List<Collider2D> dontHits = new List<Collider2D>();

    [Header("Lifetime parameters")]
    [SerializeField] private bool temporary;
    [SerializeField] private float lifetimeAfterHit;
    [SerializeField] private float maxLifetime = 5f;

    [Header("Health Impact events")]
    public UnityEvent<Collider2D> onHit;
    public UnityEvent<Collider2D, float> onHealthImpact;
    public UnityEvent onDisable;

    [Header("Debug settings")]
    [SerializeField] private bool enableDebug = true;

    private Timer timer_maxLife;

    private void Awake()
    {
        if (enableDebug)
        {
            onHit.AddListener((Collider2D other) => { Debug.Log($"Hitted {other.name}", gameObject); });
            onHealthImpact.AddListener((Collider2D other, float delta) => { Debug.Log($"Changed {other.name}'s health on {delta}", gameObject); });
        }

        timer_maxLife = new Timer(maxLifetime, () => { if (temporary) StartCoroutine(Helpers.SetActive(gameObject, false, 0, () => { onDisable.Invoke(); })); }, true);
    }

    private void Update()
    {
        timer_maxLife.Update();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.layer);
        if (!working) return;

        if (dontHits.Contains(other)) return;

        onHit.Invoke(other);

        HealthModule healthModule = other.GetComponent<HealthModule>();
        if (healthModule != null)
        {
            onHealthImpact.Invoke(other, delta);
            healthModule.ChangeHealth(delta);
        }

        if (temporary) StartCoroutine(Helpers.SetActive(gameObject, false, lifetimeAfterHit, () => { onDisable.Invoke(); }));
        working = false;
    }

    private void OnEnable()
    {
        working = true;
        timer_maxLife.Reset();
    }
}
