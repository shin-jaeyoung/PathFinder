using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PortalSlot : MonoBehaviour , IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI portalNameText;

    private ResistPortal portal;

    public void Init(ResistPortal portalData)
    {
        portal = portalData;
        portalNameText.text = portalData.PortalName;
    }
    public int GetID()
    {
        return portal.PortalID;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.MovePlayer(portal.arrivalTarget.position);
        portal.ChangeScene(portal.whereScene);
    }

}
