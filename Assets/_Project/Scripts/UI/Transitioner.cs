using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Transitioner : MonoBehaviour
{
    public Image image;

    public UnityEvent onComplete = new();

    public static Transitioner instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError($"There is more than one {this.GetType().Name} in the scene");
        }
        instance = this;
    }

    public void RightLeft(float time) 
    {
        transform.localPosition = new Vector3 (image.rectTransform.sizeDelta.x, 0f, 0f);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        transform.LeanMoveLocalX(-image.rectTransform.sizeDelta.x, time).setOnComplete(() => { image.color = new Color(image.color.r, image.color.g, image.color.b, 0); onComplete.Invoke(); });
    }

    public void RightCenter(float time)
    {
        transform.localPosition = new Vector3(image.rectTransform.sizeDelta.x, 0f, 0f);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        transform.LeanMoveLocalX(0, time / 2f).setOnComplete(() => { onComplete.Invoke(); });
    }

    public void CenterLeft(float time)
    {
        transform.LeanMoveLocalX(-image.rectTransform.sizeDelta.x, time / 2f).setOnComplete(() => { image.color = new Color(image.color.r, image.color.g, image.color.b, 0); onComplete.Invoke(); });
    }

    public void LeftRight(float time)
    {
        transform.localPosition = new Vector3(-image.rectTransform.sizeDelta.x, 0f, 0f);
        image.color = new Color(-image.color.r, image.color.g, image.color.b, 1);
        transform.LeanMoveLocalX(image.rectTransform.sizeDelta.x, time).setOnComplete(() => { image.color = new Color(image.color.r, image.color.g, image.color.b, 0); onComplete.Invoke(); });
    }

    public void LeftCenter(float time)
    {
        transform.localPosition = new Vector3(-image.rectTransform.sizeDelta.x, 0f, 0f);
        image.color = new Color(-image.color.r, image.color.g, image.color.b, 1);
        transform.LeanMoveLocalX(0, time / 2f).setOnComplete(() => { onComplete.Invoke(); });
    }

    public void CenterRight(float time)
    {
        transform.LeanMoveLocalX(image.rectTransform.sizeDelta.x, time / 2f).setOnComplete(() => { image.color = new Color(image.color.r, image.color.g, image.color.b, 0); onComplete.Invoke(); });
    }
}
