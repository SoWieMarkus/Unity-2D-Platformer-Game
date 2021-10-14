using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float directionX = 0f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float moveSpeed = 7f;

    private enum MovementState
    {
        IDLEING,
        RUNNING,
        JUMPING,
        FALLING
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        directionX = Input.GetAxisRaw("Horizontal");

        UpdateMovement();
        UpdateAnimationState();

    }

    private void UpdateMovement()
    {
        rigidbody2D.velocity = new Vector2(moveSpeed * directionX, rigidbody2D.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
        }
    }

    private void UpdateAnimationState()
    {

        MovementState state;

        if (directionX < 0f)
        {
            state = MovementState.RUNNING;
            spriteRenderer.flipX = true;
        }
        else if (directionX > 0f)
        {
            spriteRenderer.flipX = false;
            state = MovementState.RUNNING;
        }
        else
        {
            state = MovementState.IDLEING;
        }

        if (rigidbody2D.velocity.y > .1f)
        {
            state = MovementState.JUMPING;
        }
        else if (rigidbody2D.velocity.y < -.1f)
        {
            state = MovementState.FALLING;
        }


        animator.SetInteger("movementState", (int)state);

    }
}
