using System;
using System.Collections.Generic;
using UnityEngine;

public class HitscanBullet : MonoBehaviour
{
    [SerializeField] private float updateCooldown = 0.1f;

    private Transform myPool;
    private HealthImpacter2D healthImpacter;
    private LineRenderer lineRenderer;
    private Transform child;

    private Vector3 direction;

    private Timer timer_updatePositions;

    private void Awake()
    {
        myPool = transform.parent;
        child = transform.GetChild(0);
        lineRenderer = GetComponent<LineRenderer>();
        healthImpacter = child.gameObject.GetComponent<HealthImpacter2D>();
        timer_updatePositions = new Timer(updateCooldown, () => { UpdatePositions(); }, true);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        timer_updatePositions.Update();
    }

    public void Init(Material material, Transform activeBulletPool, Vector3 initPosition, Vector3 endPosition, float damage, List<Collider2D> dontHits)
    {
        lineRenderer.material = material;
        direction = endPosition - initPosition;
        lineRenderer.positionCount = Mathf.CeilToInt(direction.magnitude / 0.5f);
        lineRenderer.SetPosition(0, initPosition);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, endPosition);
        transform.SetParent(activeBulletPool);
        transform.position = initPosition;
        child.gameObject.SetActive(true);
        child.position = endPosition;
        healthImpacter.delta = -damage;
        healthImpacter.dontHits.AddRange(dontHits);
        healthImpacter.onDisable.AddListener(() =>
        {
            transform.SetParent(myPool);
            transform.position = myPool.position;
            child.position = transform.position;
            lineRenderer.positionCount = 0;
            transform.gameObject.SetActive(false);
            healthImpacter.dontHits.Clear();
            healthImpacter.onDisable.RemoveAllListeners();
        });

        UpdatePositions();
    }

    private void UpdatePositions() 
    {
        for (int i = 1; i < lineRenderer.positionCount - 1; i++) 
        {
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * 0.5f;
            lineRenderer.SetPosition(i, new Vector3(transform.position.x + direction.x / lineRenderer.positionCount * i + randomOffset.x,
                                                    transform.position.y + direction.y / lineRenderer.positionCount * i + randomOffset.y,
                                                    transform.position.z));
        }
    }

    private void OnEnable()
    {
        timer_updatePositions.Reset();
    }
}