using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuAnimations : MonoBehaviour 
{
    [SerializeField] private Image halo;
    [SerializeField] private CanvasGroup optionsGroup;
    [SerializeField] private CanvasGroup canvasGroup;

    [Space]

    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float time = 1f;

    private RadialMenu menu;

    [Space]

    [SerializeField] private bool hidden;

    [Button("Show/Hide", enabledMode: EButtonEnableMode.Playmode)]
    public void Toggle()
    {
        if (hidden)
            Show();
        else 
            Hide();
    }

    private void Awake()
    {
        menu = GetComponent<RadialMenu>();
    }

    private void Start()
    {
        if (!hidden) 
        {
            Hide();
        }
    }

    private void Show()
    {
        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, (float value) => { canvasGroup.alpha = value; }, canvasGroup.alpha, 1f, time * (1 - (canvasGroup.alpha / 1f))).setEase(animationCurve);
        LeanTween.value(gameObject, (Vector3 value) => { halo.transform.localScale = value; }, halo.transform.localScale, Vector3.one, time * (1 - (canvasGroup.alpha / 1f))).setEase(animationCurve);
        //LeanTween.value(gameObject, (float value) => { halo.fillAmount = value; }, halo.fillAmount, 1, time * (1 - (canvasGroup.alpha / 1f))).setEase(animationCurve);
        //LeanTween.value(gameObject, (float value) => { menu.complition = value; }, menu.complition, 1, time * (1 - (canvasGroup.alpha / 1f))).setEase(animationCurve);
        LeanTween.value(gameObject, (float value) => { menu.actualRadius = value; }, menu.actualRadius, menu.radius, time * (1 - (canvasGroup.alpha / 1f))).setEase(animationCurve)
            .setOnComplete(() => 
            {
                
            }
            );

        hidden = false;
        optionsGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, (float value) => { canvasGroup.alpha = value; }, canvasGroup.alpha, 0f, time * (canvasGroup.alpha / 1f)).setEase(animationCurve);
        LeanTween.value(gameObject, (Vector3 value) => { halo.transform.localScale = value; }, halo.transform.localScale, Vector3.zero, time * (canvasGroup.alpha / 1f)).setEase(animationCurve);
        //LeanTween.value(gameObject, (float value) => { halo.fillAmount = value; }, halo.fillAmount, 0, time * (canvasGroup.alpha / 1f)).setEase(animationCurve);
        //LeanTween.value(gameObject, (float value) => { menu.complition = value; }, menu.complition, 0, time * (canvasGroup.alpha / 1f)).setEase(animationCurve);
        LeanTween.value(gameObject, (float value) => { menu.actualRadius = value; }, menu.actualRadius, 0, time * (canvasGroup.alpha / 1f)).setEase(animationCurve)
            .setOnComplete(() => 
            {

            }
            );

        hidden = true;
        optionsGroup.blocksRaycasts = false;
    }
}