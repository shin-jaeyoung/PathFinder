using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    public Vector2 input;

    [Header("Interact")]
    [SerializeField] 
    private float interactRange = 1.5f;
    [SerializeField]
    private LayerMask interactLayer;

    private void Start()
    {
        player = GameManager.instance.Player;
    }

    private void Update()
    {
        if (player == null) return;
        if (player.StateMachine == null)return;

        //죽으면 키 꺼버리기
        if (player.StateMachine.CurState == player.StateMachine.stateDic[StateType.Die]) return;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        player.InputVec = input.normalized;

        //포션
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (player.StatusSystem.Stat[PlayerStatType.CurHp] > 0)
            {
                player.Potion.Use();
            }
        }

        //상호작용
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interaction();
        }
        //스킬
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.Active(0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.Active(1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.Active(2);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.Active(3);
        }

        //이동관련
        if (input.x !=0)
        {
            player.FlipSprite(input.x);
        }
        if (input.x != 0 || input.y != 0)
        {
            player.StateMachine.ChangeState(StateType.Move);
        }
        else
        {
            player.StateMachine.ChangeState(StateType.Idle);
        }
    }
    public void Interaction()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(player.transform.position, interactRange, interactLayer);

        IInteractable closest = null;
        float minDistance = float.MaxValue;

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                float distance = Vector2.Distance(player.transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }
        }

        closest?.Interact(player);
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.transform.position, interactRange);
    }
}