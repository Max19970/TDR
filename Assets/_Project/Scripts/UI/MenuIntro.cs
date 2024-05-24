using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuIntro : MonoBehaviour
{
    [SerializeField] private FMODEventRefContainer musicRefContainer;
    [SerializeField] private int doEveryNthBeat = 1;
    [SerializeField] private int speedUpAfterNBeats = 32;
    [SerializeField] private List<string> credits = new();
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image background;

    private CanvasGroup canvasGroup;

    private FMODEventContainer musicContainer;
    private UnityAction<int> onBeat;
    private int beatCount = 0;

    private Timer timer_start;

    private int animationID_textColor;

    private bool initialized;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        animationID_textColor = LeanTween.value(gameObject, (float value) => { text.color = new Color(text.color.r, text.color.g, text.color.b, value); }, 0.5f, 1f, 2f).setLoopPingPong().id;
        timer_start = new Timer(1, () => {
            musicContainer = AudioManager.instance.FindContainer(musicRefContainer.reference);
            onBeat = (int currentBeat) => { OnBeat(currentBeat); };
            musicContainer.onBeat.AddListener(onBeat);

            AudioManager.instance.Play("menu");
            timer_start.playing = false;
        }, false, 1);
    }

    private void Update()
    {
        timer_start.Update();
        if (!initialized && Input.GetKeyDown(KeyCode.Space))
        {
            timer_start.playing = true;
            text.text = string.Empty;
            LeanTween.cancel(animationID_textColor);
            text.color = Color.white;
            initialized = true;
        }
    }

    private void OnBeat(int currentBeat)
    {
        if (beatCount <= 0)
        {
            beatCount = doEveryNthBeat;

            if (doEveryNthBeat == 0) text.fontSize += 24;

            if (text.text.Equals(string.Empty))
            {
                if (credits.Count == 0)
                {
                    musicContainer.onBeat.RemoveListener(onBeat);
                    Hide();
                }
                else
                {
                    text.text = credits[0];
                    credits.RemoveAt(0);
                }
            }
            else 
            {
                text.text = string.Empty;
            }
        }
        else beatCount--;

        if ((musicContainer.timelineInfo.CurrentMusicBar * 4 + currentBeat) == speedUpAfterNBeats) doEveryNthBeat--;
    }

    private void Hide() 
    {
        background.color = Color.white;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.LeanAlpha(0f, 3f);
        Destroy(gameObject, 3f);
    }
}
