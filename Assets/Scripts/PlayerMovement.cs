using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private float directionX = 0f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float moveSpeed = 7f;

    [SerializeField] private LayerMask jumpableGround;

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
        boxCollider = GetComponent<BoxCollider2D>();
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

        if (Input.GetButtonDown("Jump") && StandingOnGround())
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
        }
    }

    private void UpdateAnimationState()
    {

        MovementState state = MovementState.IDLEING;

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

        if (rigidbody2D.velocity.y > .1f)
        {
            state = MovementState.JUMPING;
        }
        else if (rigidbody2D.velocity.y < -.1f)
        {
            Debug.Log(rigidbody2D.velocity.y + "Ich falle angeblich");

            state = MovementState.FALLING;
        }

  

        animator.SetInteger("movementState", (int)state);

    }

    private bool StandingOnGround()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
