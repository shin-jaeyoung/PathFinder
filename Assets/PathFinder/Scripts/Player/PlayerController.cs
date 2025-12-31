using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f; 

    private Rigidbody2D rb;
    private Vector2 inputVec;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true;
    }

    private void Update()
    {
        MovementInput();
    }

    private void FixedUpdate()
    {
        rb.velocity = inputVec * speed;
    }

    private void MovementInput()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        if (inputVec.magnitude > 0)
        {
            inputVec = inputVec.normalized;
        }
    }
}