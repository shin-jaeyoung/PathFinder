using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ShopSlotUI : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public int slotIndex;

    public event Action OnPointerEntered;
    public event Action OnPointerExitted;
    public event Action OnPointerClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClicked?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEntered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitted?.Invoke();
    }
}
