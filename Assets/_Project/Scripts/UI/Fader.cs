using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Fader : MonoBehaviour
{
    public Image image;

    public UnityEvent onComplete = new();

    public static Fader instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError($"There is more than one {this.GetType().Name} in the scene");
        }
        instance = this;
    }

    public void Darken(float time) 
    {
        LeanTween.value(gameObject, (float value) => { image.color = new Color(image.color.r, image.color.g, image.color.b, value); }, 0f, 1f, time).setOnComplete(() => { onComplete.Invoke(); });
    }

    public void Lighten(float time)
    {
        LeanTween.value(gameObject, (float value) => { image.color = new Color(image.color.r, image.color.g, image.color.b, value); }, 1f, 0f, time).setOnComplete(() => { onComplete.Invoke(); });
    }
}
