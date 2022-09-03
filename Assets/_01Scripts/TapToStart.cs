using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class TapToStart : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] UnityEvent OnTapDown;
    public void OnPointerDown(PointerEventData eventData)
    {
        OnTapDown?.Invoke();
    }

   
}
