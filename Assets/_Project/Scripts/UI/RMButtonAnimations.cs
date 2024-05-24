using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RMButtonAnimations : MonoBehaviour
{
    public Image tintImage;

    private int animationID_scale;

    public void OnCancel()
    {
        LeanTween.cancel(animationID_scale);

        animationID_scale = transform.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).id;
    }

    public void OnDeselect()
    {
        LeanTween.cancel(animationID_scale);

        animationID_scale = transform.LeanScale(Vector3.one, 0.1f).id;
    }

    public void OnSelect()
    {
        LeanTween.cancel(animationID_scale);

        animationID_scale = transform.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).id;
    }

    public void OnSubmit()
    {
        LeanTween.cancel(animationID_scale);

        animationID_scale = transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.05f).setOnComplete(() => { animationID_scale = transform.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.05f).id; }).id;
    }
}
