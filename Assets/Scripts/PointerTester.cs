using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PointerTester : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onClickEvents;
    public void OnPointerDown(PointerEventData eventData)
    {
        onClickEvents.Invoke();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(this.gameObject.name + " Was UnClicked.");
    }
}
