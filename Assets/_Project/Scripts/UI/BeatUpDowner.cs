using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatUpDowner : MonoBehaviour
{
    [SerializeField] private FMODEventRefContainer musicRefContainer;
    [SerializeField] private AnimationCurve scaleCurve;

    private FMODEventContainer musicContainer;
    private float time;

    private UnityAction<int> onBeat;

    private void Start()
    {
        musicContainer = AudioManager.instance.FindContainer(musicRefContainer.reference);
        musicContainer.eventInstance.getUserData(out IntPtr userData);

        onBeat = (int currentBeat) => { OnBeat(currentBeat); };
        musicContainer.onBeat.AddListener(onBeat);
    }

    private void Update()
    {
        time = 60f / musicContainer.timelineInfo.currentTempo;
    }

    private void OnBeat(int currentBeat)
    {
        if (currentBeat != 4 && currentBeat != 2) transform.LeanMoveLocalY(transform.localPosition.y - 15f, time).setEase(scaleCurve);
        else transform.LeanMoveLocalY(transform.localPosition.y - 20f, time).setEase(scaleCurve);
    }
}