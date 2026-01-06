using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    public Vector2 input;
    private void Start()
    {
        player = GameManager.instance.Player;
    }

    private void Update()
    {
        if (player == null) return;
        if (player.StateMachine == null)return;
        
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        player.InputVec = input.normalized;

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (player.StatusSystem.Stat[PlayerStatType.CurHp] > 0)
            {
                player.Potion.Use();
            }
        }

        if (input.x !=0)
        {
            player.FlipSprite(input.x);
        }

        if (input.x != 0 || input.y != 0)
        {
            player.StateMachine.ChangeState(player.MoveState);
        }
        else
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
    }
}