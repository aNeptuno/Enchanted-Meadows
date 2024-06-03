using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region "States and animations variables"
    public enum PlayerStates {
        IDLE,
        WALK,
        RUN,
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
                case PlayerStates.RUN:
                animator.Play("Walk");
                break;
                case PlayerStates.TILING:
                animator.Play("Tiling");
                break;
                case PlayerStates.WATERING:
                animator.Play("Watering");
                break;
            }
        }
    }

    public PlayerStates GetCurrentState {get => currentState;}
    public float moveSpeed = 2f;
    private float runningSpeed = 1f;
    private Vector2 moveInput = Vector2.zero;
    private Animator animator;

    private Rigidbody2D rb;

    private PlayerInput playerInput;
    private SpriteRenderer spriteRenderer;

    private bool isWatering;
    private bool isTiling;

    public bool IsInBed = false;

    #endregion

    // -- Seed in hand
    public Crop seedInHand;
    public GameObject grabbedSeedContainer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!IsInBed)
        {
            moveInput = playerInput.actions["Walk"].ReadValue<Vector2>();
            if (moveInput != Vector2.zero)
            {
                CurrentState = PlayerStates.WALK;
                //Debug.Log("<color=yellow> Move input: "+moveInput+"</color>");
                animator.SetFloat("xMove",moveInput.x);
                animator.SetFloat("yMove",moveInput.y);
            }
            else if (!isWatering && !isTiling)
            {
                CurrentState = PlayerStates.IDLE;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && seedInHand && !CanvasController.Instance.ChestUI.activeSelf)
            {
                seedInHand = null;
                grabbedSeedContainer.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed * runningSpeed;

    }

    #region "Player actions"
    public void Running(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            CurrentState = PlayerStates.RUN;
            runningSpeed = 3f;
        }
        if (callback.canceled)
        {
            runningSpeed = 1f;
        }
    }
    public void Watering(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            CurrentState = PlayerStates.WATERING;
            isWatering = true;
        }
        if (callback.canceled)
        {
            StartCoroutine(WaitForAnimation());
            isWatering = false;
        }
    }
    public void Tiling(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            CurrentState = PlayerStates.TILING;
            isTiling = true;
        }
        if (callback.canceled)
        {
            StartCoroutine(WaitForAnimation());
            isTiling = false;
        }
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.5f);
    }
    #endregion

    #region "Seed in hand"
    public void GrabSeed(Crop seedToGrab)
    {
        seedInHand = seedToGrab;
        grabbedSeedContainer.SetActive(true);
        grabbedSeedContainer.GetComponent<SpriteRenderer>().sprite = seedToGrab.seedBagSprite;
    }

    public void RemoveSeedInHand()
    {
        seedInHand = null;
        grabbedSeedContainer.GetComponent<SpriteRenderer>().sprite = null;
    }
    #endregion

}
