using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    //key
    
    public Vector2 input;
    //mouse
    [Header("Mouse")]
    public Camera mainCamera;
    public Vector2 mousePos;
    public Vector2 mouseDir;

    [Header("Interact")]
    [SerializeField] 
    private float interactRange = 1.5f;
    [SerializeField]
    private LayerMask interactLayer;

    private void Start()
    {
        player = GameManager.instance.Player;
        // 씬에 하나뿐인 MainCamera를 자동으로 찾아 할당
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
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
            GetMouseTransform();
            player.Active(0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetMouseTransform();
            player.Active(1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetMouseTransform();
            player.Active(2);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GetMouseTransform();
            player.Active(3);
        }

        //이동관련
        if (input.x !=0)
        {
            player.FlipSprite(input.x);
        }
        if (player.Skills.CheckDashSkill() && Input.GetKeyDown(KeyCode.Space)) 
        {
            GetMouseTransform();
            float diff = mousePos.x - transform.position.x;

            if (Mathf.Abs(diff) > 0.01f) 
            {
                float lookDirX = diff < 0 ? -1f : 1f;
                
                if(player.Dash())
                {
                    player.FlipSprite(lookDirX);
                }
            }
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
    public void GetMouseTransform()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        //이거 카메라 프로젝션이 orthor면 size값이랑 transform의 z값이랑 거리 맞춰줘야함 그래야 오차 안생김
        
        mousePos = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        mouseDir = (mousePos - (Vector2)transform.position).normalized;
    }


    private void OnDrawGizmosSelected()
    {
        if (player == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.transform.position, interactRange);
    }
}