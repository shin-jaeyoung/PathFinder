using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : Portal
{
    [SerializeField]
    private SceneType goScene;

    public SceneType GoScene => goScene;
    public override void Teleport(Player player)
    {
        if (GameManager.instance != null && GameManager.instance.Player != null)
        {
            ChangeScene(goScene);
            player.transform.position = arrival.position;
        }
    }
}
