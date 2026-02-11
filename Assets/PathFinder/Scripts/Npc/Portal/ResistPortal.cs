using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistPortal : Portal
{
    [SerializeField]
    protected int portalID;
    [SerializeField]
    protected string portalName;
    [SerializeField]
    protected SceneType whereisIt;

    public int PortalID => portalID;
    public string PortalName => portalName;
    public SceneType whereScene => whereisIt;


    public override void Teleport(Player player)
    {
        if(GameManager.instance.isResisPortal(portalID))
        {
            UIManager.Instance.ShowUI(UIType.Portal);
        }
        else
        {
            if(GameManager.instance.ResistPortal(portalID, this))
            {
                GlobalEvents.Notify($"전송석에 {portalName}이 등록 되었습니다.", 4f);
            }
        }
    }
}
