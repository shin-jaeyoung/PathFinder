using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPortal : Portal
{
    public override void Teleport(Player player)
    {
        if(player != null)
        {
            player.transform.position = arrival.position;
        }
    }
}
