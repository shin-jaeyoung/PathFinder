using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IDropHandler
{
    [SerializeField]
    protected Image icon;
    protected RectTransform iconRect;
    protected Vector2 originPos;

    protected CanvasGroup group;
    protected Player player;

    [SerializeField]
    protected Transform dragParent;
    protected Transform originalParent;
    //property
    public Image Icon => icon;

    private void Awake()
    {
        iconRect = icon.GetComponent<RectTransform>();
        group = icon.GetComponentInChildren<CanvasGroup>();
        originPos = iconRect.anchoredPosition;
        player = GameManager.instance.Player;
        originalParent = icon.transform.parent;

    }
  
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (icon.sprite == null) return;
        group.blocksRaycasts = false;
        if (dragParent != null)
            icon.transform.SetParent(dragParent);

        icon.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (icon.sprite == null) return;
        Canvas canvas = GetComponentInParent<Canvas>();
        iconRect.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
        
        group.blocksRaycasts = true;
        icon.transform.SetParent(originalParent);
        iconRect.anchoredPosition = Vector2.zero;
        if (icon.sprite != null)
        {
            UpdateUI();
        }
    }
    public abstract void OnDrop(PointerEventData eventData);
    public abstract void UpdateUI();
}
