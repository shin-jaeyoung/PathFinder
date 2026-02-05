using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalUI : MonoBehaviour
{

    [SerializeField]
    private PortalListView view;

    private void Start()
    {
        GameManager.instance.OnResistPortal += UpdatePortal;
        UpdatePortal();
    }

    private void OnDestroy()
    {
        GameManager.instance.OnResistPortal -= UpdatePortal;
    }
    public void UpdatePortal()
    {
        view.UpdatePortalList();
    }
}
