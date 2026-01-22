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
    public void GetMouseTransform()
    {
        // 1. 마우스의 화면 좌표(Pixel)를 가져옴
        Vector3 mouseScreenPos = Input.mousePosition;

        // 2. 직교 카메라에서는 Z값에 카메라와 캐릭터 사이의 거리를 넣어줘야 정확한 월드 좌표가 나옵니다.
        // 보통 카메라의 Z가 -10이고 캐릭터가 0이라면 거리는 10입니다.
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        // 3. 화면 좌표를 월드 좌표로 변환
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        // 4. 좌표 저장 및 방향 계산
        // Z값은 혹시 모를 오차를 위해 캐릭터와 동일하게 맞춤
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