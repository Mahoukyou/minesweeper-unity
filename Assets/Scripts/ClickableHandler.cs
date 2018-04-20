using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ClickableHandler : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnLeftClick;
    public UnityEvent OnRightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                OnLeftClick.Invoke();
                break;

            case PointerEventData.InputButton.Right:
                OnRightClick.Invoke();
                break;
        }
    }
}
