using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class ScenePortal : Portal
{
    [SerializeField]
    protected SceneType goScene;
    public override void Teleport(Player player)
    {
        if (GameManager.instance != null && GameManager.instance.Player != null)
        {
            ChangeScene(goScene);
            player.transform.position = arrival.position;
        }
    }
}
