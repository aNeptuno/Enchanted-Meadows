using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates {
        IDLE,
        WALK,
        TILING,
        WATERING
    }

    PlayerStates currentState;
    PlayerStates CurrentState {
        set {
            currentState = value;

            switch(currentState)
            {
                case PlayerStates.IDLE:
                animator.Play("Idle");
                break;
                case PlayerStates.WALK:
                animator.Play("Walk");
                break;
            }
        }
    }
    public float moveSpeed = 2f;
    private float runningSpeed = 1f;
    private Vector2 moveInput = Vector2.zero;
    private Animator animator;

    private Rigidbody2D rb;

    private PlayerInput playerInput;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = playerInput.actions["Walk"].ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            CurrentState = PlayerStates.WALK;
            //Debug.Log("<color=yellow> Move input: "+moveInput+"</color>");
            animator.SetFloat("xMove",moveInput.x);
            animator.SetFloat("yMove",moveInput.y);
        }
        else
        {
            CurrentState = PlayerStates.IDLE;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed * runningSpeed;

    }

    // -- Player actions
    public void Running(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            runningSpeed = 3f;
        }
        if (callback.canceled)
        {
            runningSpeed = 1f;
        }
    }
    public void Watering(InputAction.CallbackContext callback)
    {

    }

    public void Tiling()
    {

    }
}
