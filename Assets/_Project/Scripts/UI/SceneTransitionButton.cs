using UnityEngine.Events;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    [Scene] [SerializeField] private int levelToLoad;
    [SerializeField] private CanvasGroup currentMenu;
    [SerializeField] private float time;
    private Fader fader;

    private void Start()
    {
        fader = Fader.instance;
    }

    public void OnClick()
    {
        if (currentMenu) currentMenu.blocksRaycasts = false;
        AudioManager.instance.StopAll();

        UnityAction action1 = () => { SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single); };
        action1 += () => { fader.onComplete.RemoveListener(action1); };
        fader.onComplete.AddListener(action1);
        fader.Darken(time);
    }
}