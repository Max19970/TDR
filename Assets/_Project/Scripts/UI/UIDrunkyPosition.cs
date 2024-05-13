using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDrunkyPosition : MonoBehaviour
{
    [SerializeField] private float frequency = 5f;
    [SerializeField] private float amplitude = 1f;

    private Vector3 actualOffset;
    private Vector3 moveVelocity;

    private Timer pointTimer;

    private void Awake()
    {
        pointTimer = new Timer(frequency, () => { UpdatePoint(); });
    }

    private void Update()
    {
        UpdateTimers();
        ChangePosition();
    }

    private void UpdateTimers() 
    {
        pointTimer.Update();
    }

    private void UpdatePoint() 
    {
        actualOffset = (Vector3)(Random.insideUnitCircle * amplitude);
    }

    private void ChangePosition() 
    {
        transform.position = Vector3.SmoothDamp(transform.position, transform.parent.position + actualOffset, ref moveVelocity, frequency);
    }
}
