using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RMOption : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void ReplaceIcon(Sprite newIcon) 
    {
        icon.sprite = newIcon;
        if (!newIcon) icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0);
        else icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1);
    }
}
