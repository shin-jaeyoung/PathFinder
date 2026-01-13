using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using TMPro;
public class ShopInvenSlotUI : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public int slotIndex;
    public Image image;
    public TextMeshProUGUI count;

    public event Action OnPointerEntered;
    public event Action OnPointerExitted;
    public event Action OnPointerClicked;


    public void RefreshUI(Sprite sprite,int count)
    {
        image.gameObject.SetActive(sprite != null);

        this.image.sprite = sprite;
        if(count > 1)
        {
            this.count.text = count.ToString();
        }
        else
        {
            this.count.gameObject.SetActive(false);
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
}
