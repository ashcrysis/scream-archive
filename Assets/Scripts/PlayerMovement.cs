using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Grounded = Animator.StringToHash("isGrounded");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    private bool _isSprinting;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform[] groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    private Vector2 _moveInput;
    [FormerlySerializedAs("_isGrounded")] [SerializeField] private bool isGrounded;
    private Vector3 _moveDirection;
    protected Rigidbody Rb;
    protected PlayerInput PlayerInput;
    protected Animator Animator;
        
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();
        Animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (!PlayerInput) return;
            
        PlayerInput.actions["Move"].performed += OnMoveInput;
        PlayerInput.actions["Move"].canceled += OnMoveInput;
         
        PlayerInput.actions["Sprint"].performed += SetSprinting;
        PlayerInput.actions["Sprint"].canceled += ResetSprinting;
    }

    void OnDestroy()
    {
        if (PlayerInput != null)
        {
            PlayerInput.actions["Sprint"].performed -= SetSprinting;
            PlayerInput.actions["Sprint"].canceled -= ResetSprinting;
            PlayerInput.actions["Move"].performed -= OnMoveInput;
            PlayerInput.actions["Move"].canceled -= OnMoveInput;
        }
    }

    private void SetSprinting(InputAction.CallbackContext ctx) => _isSprinting = true;
    private void ResetSprinting(InputAction.CallbackContext ctx) => _isSprinting = false;
        

    protected void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        isGrounded = IsGrounded();
        Vector3 bodyForward = transform.forward;
        Vector3 bodyRight = transform.right;
        bodyForward = Vector3.ProjectOnPlane(bodyForward, transform.up).normalized;
        bodyRight = Vector3.ProjectOnPlane(bodyRight, transform.up).normalized;

        _moveDirection = bodyForward * _moveInput.y + bodyRight * _moveInput.x;
        //SetAnimations();
    }

    void FixedUpdate()
    {
           
        float speed = _isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector3 move = _moveDirection * (speed * Time.fixedDeltaTime);
            
        Rb.MovePosition(Rb.position + move);
    }

    private void SetAnimations()
    {
        Animator.SetBool(IsMoving, _moveDirection.sqrMagnitude > 0);
        Animator.SetBool(Grounded, isGrounded);
         
        Animator.SetBool(IsFalling, false);
        Animator.SetBool(IsRunning, _isSprinting && _moveDirection.sqrMagnitude > 0);
    }

    private bool IsGrounded()
    {
        foreach (Transform checkPoint in groundCheck)
        {
            if (Physics.CheckSphere(checkPoint.position, groundCheckRadius, groundLayer))
            {
                return true;
            }
        }
        return false;
    }
}