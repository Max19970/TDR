using UnityEngine;

public class Enemy1 : Enemy 
{
    protected override void Move()
    {
        rb.AddForce(moveDirectionNormalized * speed * rb.mass, ForceMode2D.Force);
    }
}