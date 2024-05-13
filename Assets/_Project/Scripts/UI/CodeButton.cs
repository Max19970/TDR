using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodeButton : Selectable, ISubmitHandler, ICancelHandler, IPointerClickHandler
{
    public UnityEvent<BaseEventData> onSelect;
    public UnityEvent<BaseEventData> onDeselect;
    public UnityEvent<BaseEventData> onSubmit;
    public UnityEvent<BaseEventData> onCancel;
    public UnityEvent onSetActive;
    public UnityEvent onSetInactive;

    public override void OnSelect(BaseEventData eventData = null)
    {
        //Debug.Log("I'm selected", gameObject);
        onSelect.Invoke(eventData);
    }

    public override void OnDeselect(BaseEventData eventData = null)
    {
        //Debug.Log("I'm deselected", gameObject);
        onDeselect.Invoke(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData = null)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData = null)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerClick(PointerEventData eventData = null) 
    {
        OnSubmit();
    }

    public void OnSubmit(BaseEventData eventData = null)
    {
        Debug.Log("I'm submited", gameObject);
        onSubmit.Invoke(eventData);
    }

    public void OnCancel(BaseEventData eventData = null)
    {
        //Debug.Log("I'm canceled", gameObject);
        onCancel.Invoke(eventData);
    }

    //protected override void OnEnable()
    //{
    //    //Debug.Log("I'm enabled", gameObject);
    //    onSetActive.Invoke();
    //}
    //
    //protected override void OnDisable()
    //{
    //    //Debug.Log("I'm disabled", gameObject);
    //    onSetInactive.Invoke();
    //}
}