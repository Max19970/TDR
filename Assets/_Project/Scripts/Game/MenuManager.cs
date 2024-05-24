using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool showIntro;
    [ShowIf("showIntro")]
    [SerializeField] private GameObject intro;

    private Transitioner transitioner;
    private Fader fader;

    private void Awake()
    {
        showIntro = StaticGameData.showIntroInMenu;
    }

    private void Start()
    {
        transitioner = Transitioner.instance;
        fader = Fader.instance;

        fader.Darken(0f);
        fader.Lighten(2f);

        if (showIntro)
        { 
            intro.SetActive(true);
            StaticGameData.showIntroInMenu = false;
        }
        else
        {
            FMODEventContainer container = AudioManager.instance.Play("menu");
            container.SetParameter("State", 1);
        }
    }
}
