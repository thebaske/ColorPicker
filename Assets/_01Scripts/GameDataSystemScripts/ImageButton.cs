using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ImageButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnPointerDownHandler;
    public UnityEvent OnPointerUpHandler;
    private bool interactive = true;

    public void DisableMe()
    {
        interactive = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactive) return;
        
        OnPointerDownHandler?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactive) return;
        OnPointerUpHandler?.Invoke();
    }
}
