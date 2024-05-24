using UnityEngine;

public class Enemy2 : Enemy
{
    private float angle = 90f;

    protected override void Start()
    {
        base.Start();
        ChangeAngle();
    }

    protected override void Move()
    {
        rb.AddForce(moveDirectionNormalized * speed * rb.mass, ForceMode2D.Force);
        rb.AddForce(Quaternion.Euler(0, 0, angle) * moveDirectionNormalized * speed * 4 * rb.mass, ForceMode2D.Force);
    }

    private void ChangeAngle() 
    {
        LeanTween.value(gameObject, (float value) => { angle = value; }, 90, -90, 0.5f).setLoopPingPong();
    }
}