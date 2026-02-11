using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PortalSlot : MonoBehaviour , IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI portalNameText;

    private PortalData portalData;
    public void Init(PortalData portalData)
    {
        this.portalData = portalData;
        portalNameText.text = portalData.Name;
    }
    public int GetID()
    {
        return portalData.ID;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.MovePlayer(portalData.Position);
        SceneManager.LoadScene(portalData.Scene.ToString());
        UIManager.Instance.HideUI(UIType.Portal);
    }

}
