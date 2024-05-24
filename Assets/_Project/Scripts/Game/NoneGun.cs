using UnityEngine;

public class NoneGun : Gun 
{
    public override void Fire()
    {
        
    }

    protected override void Rotate()
    {
        rotateAnchor.rotation = Quaternion.Slerp(rotateAnchor.rotation, Quaternion.AngleAxis(Vector2.SignedAngle(Vector3.up, target.position - rotateAnchor.position), Vector3.forward), 1f / rotationSpeed * Time.deltaTime);
    }
}