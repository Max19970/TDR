using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleButtonAnimations : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private Vector3 initScale;

    private int animationID_scale;
    private int animationID_textColor;

    private void Awake()
    {
        initScale = transform.localScale;
    }

    public void Scale(float scale) 
    {
        LeanTween.cancel(animationID_scale);
        animationID_scale = transform.LeanScale(initScale * scale, 0.1f).id;
    }

    public void Scale(float scale, float time)
    {
        LeanTween.cancel(animationID_scale);
        animationID_scale = transform.LeanScale(initScale * scale, time).id;
    }

    public void Bounce(float scale)
    {
        LeanTween.cancel(animationID_scale);
        Vector3 myScale = transform.localScale;
        animationID_scale = transform.LeanScale(myScale * scale, 0.05f).setOnComplete(() => { animationID_scale = transform.LeanScale(myScale, 0.05f).id; }).id;
    }

    public void Bounce(float scale, float time)
    {
        LeanTween.cancel(animationID_scale);
        Vector3 myScale = transform.localScale;
        animationID_scale = transform.LeanScale(myScale * scale, time / 2f).setOnComplete(() => { animationID_scale = transform.LeanScale(myScale, time / 2f).id; }).id;
    }

    public void FadeIn(float time) 
    {
        canvasGroup.LeanAlpha(1f, time);
        canvasGroup.blocksRaycasts = true;
    }

    public void FadeOut(float time)
    {
        canvasGroup.LeanAlpha(0f, time);
        canvasGroup.blocksRaycasts = false;
    }

    public void PlaySound(string name) 
    {
        AudioManager.instance.PlayOneShot(name, transform.position);
    }
}
