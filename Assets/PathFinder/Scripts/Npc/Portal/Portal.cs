using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    protected void ChangeScene(SceneType sceneType)
    {
        if (GameManager.instance != null && GameManager.instance.Player != null)
        {
            SceneManager.LoadScene(sceneType.ToString());
            GameManager.instance.SetScene(sceneType);
        }
    }
}
