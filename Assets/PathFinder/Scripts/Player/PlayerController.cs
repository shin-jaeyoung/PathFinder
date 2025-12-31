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
        
        if(input.x !=0)
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