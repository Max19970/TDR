using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGameButton : MonoBehaviour
{
    [SerializeField] private LazyRotator rotator;
    [SerializeField] private LazyScaler scaler;
    [SerializeField] private SimpleButtonAnimations animations;

    public void OnClick() 
    {
        EnemySpawner.instance.Begin();
        AudioManager.instance.Stop("prepare");
        AudioManager.instance.Play($"battle{Random.Range(1, 4)}");

        rotator.Stop();
        scaler.Stop();
        animations.Scale(transform.localScale.x + 2f, 3f);
        animations.FadeOut(3f);
    }
}
