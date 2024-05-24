using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int moneyCount;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private int healthCount;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private CanvasGroup gameoverMenu;
    [SerializeField] private CanvasGroup victoryMenu;

    private Transitioner transitioner;
    private Fader fader;

    private int moneyCountToShow;
    private int healthCountToShow;

    private int animationID_money;
    private int animationID_health;

    private bool ended = false;

    public static LevelManager instance { get; private set; }
    public int MoneyCount { get { return moneyCount; } set { LeanTween.cancel(animationID_money); animationID_money = LeanTween.value(gameObject, (float v) => { moneyCountToShow = (int)v; }, moneyCount, value, 0.2f).id; moneyCount = value; } }
    public int HealthCount { get { return healthCount; } set { if (value < healthCount) AudioManager.instance.PlayOneShot("hurt", transform.position); LeanTween.cancel(animationID_health); animationID_money = LeanTween.value(gameObject, (float v) => { healthCountToShow = (int)v; }, healthCount, value, 0.2f).id; healthCount = value; } }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError($"There is more than one {this.GetType().Name} in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        MoneyCount = moneyCount;
        HealthCount = healthCount;

        transitioner = Transitioner.instance;
        fader = Fader.instance;

        fader.Darken(0);
        fader.Lighten(0.35f);

        AudioManager.instance.Play("prepare");
}

    private void Update()
    {
        moneyText.text = moneyCountToShow.ToString();
        healthText.text = healthCountToShow.ToString();

        if (!ended && healthCount <= 0 && !EnemySpawner.instance.ended)
        {
            ended = true;
            EnemySpawner.instance.Stop();
            AudioManager.instance.StopAll();
            gameoverMenu.blocksRaycasts = true;
            gameoverMenu.LeanAlpha(1f, 1f);
            AudioManager.instance.Play("gameover");
        }

        if (!ended && (Input.GetKeyDown(KeyCode.W) || healthCount > 0 && EnemySpawner.instance.ended)) 
        {
            ended = true;
            EnemySpawner.instance.Stop();
            AudioManager.instance.StopAll();
            victoryMenu.blocksRaycasts = true;
            victoryMenu.LeanAlpha(1f, 1f);
            AudioManager.instance.Play("victory");
        }
    }
}
