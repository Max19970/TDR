using UnityEngine.EventSystems;

public interface ICodeButton 
{
    void OnSelect(BaseEventData eventData);
    void OnDeselect(BaseEventData eventData);
    void OnSubmit(BaseEventData eventData);
    void OnCancel(BaseEventData eventData);
    void OnSetActive();
    void OnSetInactive();
}