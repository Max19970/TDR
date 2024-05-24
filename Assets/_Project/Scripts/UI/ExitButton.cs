using UnityEngine.Events;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private float time;
    private Transitioner transitioner;

    private void Start()
    {
        transitioner = Transitioner.instance;
    }

    public void OnClick()
    {
        UnityAction action1 = () => { Debug.Log("Exit"); Application.Quit(); };
        action1 += () => { transitioner.onComplete.RemoveListener(action1); };
        transitioner.onComplete.AddListener(action1);
        transitioner.RightCenter(time / 2f);
    }
}