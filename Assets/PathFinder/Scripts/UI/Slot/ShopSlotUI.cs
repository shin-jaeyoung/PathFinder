using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopSlotUI : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public Item item;
    public Image image;
    public event Action OnPointerEntered;
    public event Action OnPointerExitted;
    public event Action OnPointerClicked;

    public bool IsEmpty()
    {
        if (item == null) return true;

        return false;
    }
    public void RefreshUI()
    {
        if (item != null)
        {
            image.gameObject.SetActive(item.Data.Sprite != null);
            image.sprite = item.Data.Sprite;
        }
    }
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

    private void OnDestroy()
    {
        OnPointerEntered = null;
        OnPointerExitted = null;
        OnPointerClicked = null;
    }
}
