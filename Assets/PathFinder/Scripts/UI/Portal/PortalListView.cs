using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalListView : MonoBehaviour
{
    [SerializeField]
    private PortalSlot slot;

    private List<PortalSlot> resistPortals = new List<PortalSlot>();
    public void UpdatePortalList()
    {
        //GameManager.instance.resistedPortal[0];
        
        foreach (KeyValuePair<int,PortalData> pair in GameManager.instance.resistedPortal)
        {
            if (FindID(pair.Key))
            {
                continue;
            }

            PortalSlot portalSlot = Instantiate(slot, transform);
            portalSlot.Init(pair.Value);

            resistPortals.Add(portalSlot);
        }
    }
    private bool FindID(int id)
    {
        foreach( PortalSlot portalSlot in resistPortals )
        {
            if (portalSlot.GetID() == id) return true;
        }
        return false;
    }
}
