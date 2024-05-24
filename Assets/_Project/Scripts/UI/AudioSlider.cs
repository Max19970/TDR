using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private BUS bus;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.value = AudioManager.instance.GetVolume((int)bus);
    }

    public void ChangeVolume(float value) 
    {
        AudioManager.instance.SetVolume((int)bus, value);
    }
}
