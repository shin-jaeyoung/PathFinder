using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToTown : Portal
{

    public override void Teleport(Player player)
    {
        if(GameManager.instance!=null && GameManager.instance.Player != null)
        {
            if(GameManager.instance.CurScene != SceneType.Town)
            {
                ChangeScene(SceneType.Town);
            }
            player.transform.position = arrival.position;
        }
    }
}
