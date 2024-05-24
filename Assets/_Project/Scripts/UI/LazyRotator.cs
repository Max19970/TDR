using UnityEngine;

public class LazyRotator : MonoBehaviour 
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float time;

    private void Start()
    {
        transform.LeanRotateZ(10, time).setEase(curve).setLoopClamp();
    }

    public void Stop() 
    {
        LeanTween.cancel(gameObject);
    }
}