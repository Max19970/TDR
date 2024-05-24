using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using DentedPixel;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)] public float masterVolume = 1;
    [Range(0, 1)] public float musicVolume = 1;
    //[Range(0, 1)] public float ambienceVolume = 1;
    [Range(0, 1)] public float sfxVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    //private Bus ambienceBus;
    private Bus sfxBus;

    [Header("Tracks")]
    public List<FMODEventContainer> events = new();
    public List<FMODEventRefContainer> references = new();

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one AudioManager object in the scene.");
        }
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        //ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        Load();
    }

    private void Load()
    {
        foreach (FMODEventRefContainer refContainer in references) 
        {
            CreateInstance(refContainer.name, refContainer.reference);
        }
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayOneShot(string name, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(FindContainer(name).eventReference, worldPos);
    }

    public FMODEventContainer CreateInstance(string name, EventReference sound)
    {
        foreach (FMODEventContainer container in events)
        {
            if (container.name.Equals(name)) return container;
        }

        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        FMODEventContainer newContainer = new FMODEventContainer(name, eventInstance, sound);

        events.Add(newContainer);
        return newContainer;
    }

    public FMODEventContainer Play(string name)
    {
        foreach (FMODEventContainer sound in events)
        {
            if (sound.name.Equals(name))
            {
                sound.Play();
                return sound;
            }
        }
        return null;
    }

    public FMODEventContainer Stop(string name)
    {
        foreach (FMODEventContainer sound in events)
        {
            if (sound.name.Equals(name))
            {
                sound.Stop();
                return sound;
            }
        }
        return null;
    }

    public void StopAll()
    {
        foreach (FMODEventContainer sound in events)
        {
            sound.eventInstance.getPlaybackState(out PLAYBACK_STATE soundState);
            if (soundState == PLAYBACK_STATE.PLAYING) sound.Stop();
        }
    }

    public void SetupEmitter(StudioEventEmitter emitter, string name)
    {
        FMODEventContainer container = FindContainer(name);
        emitter.AllowFadeout = FindContainer(emitter.EventReference).AllowFadeOut;
        emitter.EventReference = container.eventReference;
    }

    public void SetupEmitter(StudioEventEmitter emitter, EventReference reference)
    {
        FMODEventContainer container = FindContainer(reference);
        emitter.AllowFadeout = FindContainer(emitter.EventReference).AllowFadeOut;
        emitter.EventReference = container.eventReference;
    }

    public void SetupEmitter(StudioEventEmitter emitter, EventInstance instance)
    {
        FMODEventContainer container = FindContainer(instance);
        emitter.AllowFadeout = FindContainer(emitter.EventReference).AllowFadeOut;
        emitter.EventReference = container.eventReference;
    }

    public FMODEventContainer FindContainer(string name)
    {
        foreach (FMODEventContainer container in events)
        {
            if (container.name.Equals(name)) return container;
        }

        Debug.LogError($"Couldn't find a container with name {name}");

        return null;
    }

    public FMODEventContainer FindContainer(EventInstance instance)
    {
        foreach (FMODEventContainer container in events)
        {
            if (container.eventInstance.Equals(instance)) return container;
        }

        Debug.LogError($"Couldn't find a container with instance {instance}");

        return null;
    }

    public FMODEventContainer FindContainer(EventReference reference) 
    {
        foreach (FMODEventContainer container in events) 
        {
            if (container.eventReference.Equals(reference)) return container;
        }

        Debug.LogError($"Couldn't find a container with reference {reference}");

        return null;
    }

    public void SetVolume(int id, float value)
    {
        switch (id) 
        {
            case 0:
                masterBus.setVolume(value);
                break;
            case 1:
                musicBus.setVolume(value);
                break;
            case 2:
                sfxBus.setVolume(value);
                break;
            default:
                break;
        }
    }

    public float GetVolume(int id)
    {
        float value = 1f;
        switch (id)
        {
            case 0:
                masterBus.getVolume(out value);
                return value;
            case 1:
                musicBus.getVolume(out value);
                return value;
            case 2:
                sfxBus.getVolume(out value);
                return value;
            default:
                return value;
        }
    }

    public void SetGlobalParameter(string parameterName, float value) 
    {
        RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
    }

    public void SetParameter(string eventName, string parameterName, float value)
    {
        FindContainer(eventName).SetParameter(parameterName, value);
    }

    public void SetParameter(EventReference eventReference, string parameterName, float value)
    {
        FindContainer(eventReference).SetParameter(parameterName, value);
    }

    public void SetParameter(EventInstance eventInstance, string parameterName, float value)
    {
        FindContainer(eventInstance).SetParameter(parameterName, value);
    }
}

public class FMODEventContainer
{
    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        private int currentMusicBar = 0;
        public int CurrentMusicBar { get { return currentMusicBar; } set { if (value != currentMusicBar) onBar(value); currentMusicBar = value; } }
        private int currentMusicBeat = 0;
        public int CurrentMusicBeat { get { return currentMusicBeat; } set { if (value != currentMusicBeat) onBeat(value); currentMusicBeat = value; } }
        private string lastMarker = string.Empty;
        public string LastMarker { get { return lastMarker; } set { if (value != lastMarker) onMarker(value); lastMarker = value; } }
        public float currentTempo;

        public Action<int> onBeat;
        public Action<int> onBar;
        public Action<string> onMarker;
    }

    public string name;
    public FMOD.Studio.STOP_MODE stopMode;
    public EventInstance eventInstance;
    public EVENT_CALLBACK beatCallback;
    public EventReference eventReference;

    public UnityEvent<int> onBeat = new();
    public UnityEvent<int> onBar = new();
    public UnityEvent<string> onMarker = new();

    public TimelineInfo timelineInfo;
    public GCHandle timelineHandle;

    public bool AllowFadeOut => stopMode == FMOD.Studio.STOP_MODE.ALLOWFADEOUT;

    public FMODEventContainer(string name, EventInstance eventInstance, EventReference reference, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
    {
        this.name = name;
        this.eventInstance = eventInstance;
        this.eventReference = reference;
        this.stopMode = stopMode;

        timelineInfo = new TimelineInfo();
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        eventInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        this.beatCallback = new EVENT_CALLBACK(BeatEventCallback);
        eventInstance.setCallback(beatCallback, EVENT_CALLBACK_TYPE.TIMELINE_BEAT | EVENT_CALLBACK_TYPE.TIMELINE_MARKER);

        timelineInfo.onBar += (int value) => { onBar.Invoke(value); };
        timelineInfo.onBeat += (int currentBeat) => { onBeat.Invoke(currentBeat); };
        timelineInfo.onMarker += (string value) => { onMarker.Invoke(value); };
    }

    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    private FMOD.RESULT BeatEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        IntPtr timelineInfoPtr;
        EventInstance instance = new EventInstance(instancePtr);
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.CurrentMusicBar = parameter.bar;
                        timelineInfo.CurrentMusicBeat = parameter.beat;
                        timelineInfo.currentTempo = parameter.tempo;
                    }
                    break;
                case EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.LastMarker = parameter.name;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }

    public void Play()
    {
        eventInstance.start();
    }

    public void Stop()
    {
        eventInstance.stop(stopMode);
    }

    public void SetPaused(bool paused)
    {
        eventInstance.setPaused(paused);
    }

    public void SetParameter(string parameterName, float value) 
    {
        eventInstance.setParameterByName(parameterName, value);
    }
}


[Serializable]
public struct FMODEventRefContainer
{
    public string name;
    public EventReference reference;
}


public enum BUS
{
    MASTER = 0,
    MUSIC = 1,
    SFX = 2,
}
