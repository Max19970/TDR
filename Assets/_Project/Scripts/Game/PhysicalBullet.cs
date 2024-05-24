using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PhysicalBullet : MonoBehaviour 
{
    private Transform myPool;
    private HealthImpacter2D healthImpacter;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        myPool = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthImpacter = GetComponent<HealthImpacter2D>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Init(Sprite sprite, Transform activeBulletPool, Vector3 initPosition, Vector3 moveDirection, float speed, float damage, List<Collider2D> dontHits)
    {
        spriteRenderer.sprite = sprite;
        transform.SetParent(activeBulletPool);
        transform.position = initPosition;
        transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector3.up, moveDirection), Vector3.forward);
        rb.AddForce(moveDirection * speed, ForceMode2D.Impulse);
        healthImpacter.delta = -damage;
        healthImpacter.dontHits.AddRange(dontHits);
        healthImpacter.onDisable.AddListener(() =>
        {
            transform.SetParent(myPool);
            transform.position = myPool.position;
            rb.velocity = Vector2.zero;
            healthImpacter.dontHits.Clear();
            healthImpacter.onDisable.RemoveAllListeners();
        });
    }
}