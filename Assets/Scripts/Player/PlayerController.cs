using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    public static PlayerController Instance;

    private PlayerControls playerControls;
    public Vector2 movement;
    private Rigidbody2D rb;

    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private bool facingLeft = false;

    PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        movement = playerControls.Movement.Move.ReadValue<Vector2>(); 

        myAnimator.SetFloat("moveX", movement.x); 
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        rb.MovePosition(rb.position + movement * (playerStats.CurrentMoveSpeed * Time.deltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            facingLeft = false;
        }
    }
}
