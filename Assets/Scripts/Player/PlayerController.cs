using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float sprintSpeed = 6.5f;
    public float rotationSmoothTime = 0.12f;
    public float speedChangeRate = 10f;

    [Header("Jump And Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -15f;
    public float jumpCooldown = 0.5f;
    public float fallAnimationDelay = 0.15f;

    [Header("Ground Check")]
    public bool grounded = true;
    public float groundedOffset = -0.14f;
    public float groundedRadius = 0.28f;
    public LayerMask groundLayers = 1;

    [Header("Camera")]
    public Transform cameraTarget;
    public Camera playerCamera;
    public float mouseSensitivity = 1f;
    public float topClamp = 70f;
    public float bottomClamp = -30f;
    public bool lockCamera;

    [Header("Input")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    private CharacterController controller;
    private Animator animator;

    private float currentSpeed;
    private float animationBlend;
    private float targetRotation;
    private float rotationVelocity;
    private float verticalVelocity;
    private float cameraYaw;
    private float cameraPitch;
    private float jumpCooldownTimer;
    private float fallAnimationTimer;

    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;

    private const float TerminalVelocity = 53f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (cameraTarget == null && playerCamera != null)
        {
            cameraTarget = playerCamera.transform.parent;
        }
    }

    private void Start()
    {
        AssignAnimationIDs();

        if (cameraTarget != null)
        {
            cameraYaw = cameraTarget.rotation.eulerAngles.y;
            cameraPitch = cameraTarget.rotation.eulerAngles.x;
        }

        jumpCooldownTimer = jumpCooldown;
        fallAnimationTimer = fallAnimationDelay;
    }

    private void Update()
    {
        GroundedCheck();
        JumpAndGravity();
        Move();
    }

    private void LateUpdate()
    {
        RotateCameraTarget();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z
        );

        grounded = Physics.CheckSphere(
            spherePosition,
            groundedRadius,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );

        if (animator != null)
        {
            animator.SetBool(animIDGrounded, grounded);
        }
    }

    private void Move()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = Vector2.ClampMagnitude(input, 1f);

        bool isSprinting = Input.GetKey(sprintKey);
        float targetSpeed = isSprinting ? sprintSpeed : moveSpeed;

        if (input == Vector2.zero)
        {
            targetSpeed = 0f;
        }

        float horizontalSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
        float inputMagnitude = input.magnitude;

        if (Mathf.Abs(horizontalSpeed - targetSpeed) > 0.1f)
        {
            currentSpeed = Mathf.Lerp(
                horizontalSpeed,
                targetSpeed * inputMagnitude,
                Time.deltaTime * speedChangeRate
            );
            currentSpeed = Mathf.Round(currentSpeed * 1000f) / 1000f;
        }
        else
        {
            currentSpeed = targetSpeed;
        }

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (animationBlend < 0.01f)
        {
            animationBlend = 0f;
        }

        Vector3 inputDirection = new Vector3(input.x, 0f, input.y).normalized;

        if (input != Vector2.zero)
        {
            float cameraY = playerCamera != null ? playerCamera.transform.eulerAngles.y : 0f;
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraY;

            float rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetRotation,
                ref rotationVelocity,
                rotationSmoothTime
            );

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
        Vector3 movement = targetDirection.normalized * currentSpeed;
        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);

        if (animator != null)
        {
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);
        }
    }

    private void JumpAndGravity()
    {
        if (grounded)
        {
            fallAnimationTimer = fallAnimationDelay;

            if (animator != null)
            {
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDFreeFall, false);
            }

            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            if (Input.GetKeyDown(jumpKey) && jumpCooldownTimer <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                if (animator != null)
                {
                    animator.SetBool(animIDJump, true);
                }
            }

            if (jumpCooldownTimer >= 0f)
            {
                jumpCooldownTimer -= Time.deltaTime;
            }
        }
        else
        {
            jumpCooldownTimer = jumpCooldown;

            if (fallAnimationTimer >= 0f)
            {
                fallAnimationTimer -= Time.deltaTime;
            }
            else if (animator != null)
            {
                animator.SetBool(animIDFreeFall, true);
            }
        }

        if (verticalVelocity < TerminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void RotateCameraTarget()
    {
        if (lockCamera || cameraTarget == null)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraYaw += mouseX;
        cameraPitch -= mouseY;
        cameraPitch = ClampAngle(cameraPitch, bottomClamp, topClamp);

        cameraTarget.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = grounded
            ? new Color(0f, 1f, 0f, 0.35f)
            : new Color(1f, 0f, 0f, 0.35f);

        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z),
            groundedRadius
        );
    }
}
