using UnityEngine.Events;
using UnityEngine;

public class MenuTransitionButton : MonoBehaviour 
{
    [SerializeField] private CanvasGroup menu1;
    [SerializeField] private CanvasGroup menu2;
    [SerializeField] private float time;
    private Transitioner transitioner;

    private void Start()
    {
        transitioner = Transitioner.instance;
    }

    public void OnClick() 
    {
        menu1.blocksRaycasts = false;

        UnityAction action1 = () => { menu1.alpha = 0f; menu2.blocksRaycasts = true; menu2.alpha = 1f; transitioner.CenterLeft(time / 2f); };
        action1 += () => { transitioner.onComplete.RemoveListener(action1); };
        transitioner.onComplete.AddListener(action1);
        transitioner.RightCenter(time / 2f);
    }
}