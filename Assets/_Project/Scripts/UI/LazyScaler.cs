using UnityEngine;

public class LazyScaler : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float time;

    private Vector3 initScale;

    private void Awake()
    {
        initScale = transform.localScale;
    }

    private void Start()
    {
        transform.LeanScale(initScale * 1.1f, time / 2f).setEase(curve).setLoopClamp();
    }

    public void Stop()
    {
        LeanTween.cancel(gameObject);
    }
}