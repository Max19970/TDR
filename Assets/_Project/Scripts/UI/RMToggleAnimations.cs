using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RMToggleAnimations : MonoBehaviour
{
    public Image image;
    public Color toggledColor;
    public Color selectionColor;

    public bool toggled;

    private int animationID_scale;
    private Color currentColor;

    private void Start()
    {
        if (toggled) currentColor = toggledColor;
        else currentColor = selectionColor;
        OnDeselect();
    }

    public void OnCancel()
    {
        LeanTween.cancel(animationID_scale);

        animationID_scale = transform.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).id;
    }

    public void OnDeselect()
    {
        LeanTween.cancel(animationID_scale);

        animationID_scale = transform.LeanScale(Vector3.one, 0.1f).id;
        LeanTween.value(gameObject, (Color value) => { image.color = value; }, image.color, new Color(currentColor.r, currentColor.g, currentColor.b, 0f), 0.1f);
    }

    public void OnSelect()
    {
        LeanTween.cancel(animationID_scale);

        animationID_scale = transform.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).id;
        LeanTween.value(gameObject, (Color value) => { image.color = value; }, image.color, new Color(currentColor.r, currentColor.g, currentColor.b, 1f), 0.1f);

        AudioManager.instance.PlayOneShot("menumove", transform.position);
    }

    public void OnSubmit()
    {
        toggled = !toggled;

        LeanTween.cancel(animationID_scale);

        animationID_scale = transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.05f).setOnComplete(() => { animationID_scale = transform.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.05f).id; }).id;
        if (toggled) currentColor = toggledColor;
        else currentColor = selectionColor;

        AudioManager.instance.PlayOneShot("menuconfirm", transform.position);
    }
}
