using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PortalData
{
    public int ID;
    public string Name;
    public SceneType Scene;
    public Vector3 Position;

    public PortalData(ResistPortal portal)
    {
        ID = portal.PortalID;
        Name = portal.PortalName;
        Scene = portal.whereScene;
        Position = portal.transform.position;
    }
}
public abstract class Portal : MonoBehaviour , IInteractable
{
    [SerializeField]
    protected Transform arrival;

    public Transform arrivalTarget => arrival;

    public void Interact(Player player)
    {
        Teleport(player);
    }

    public abstract void Teleport(Player player);
    public void ChangeScene(SceneType sceneType)
    {
        if (GameManager.instance != null && GameManager.instance.Player != null)
        {
            SceneManager.LoadScene(sceneType.ToString());
            GameManager.instance.SetScene(sceneType);
        }
    }
}
