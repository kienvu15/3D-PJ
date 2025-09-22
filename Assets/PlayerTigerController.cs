using UnityEngine;
using PlayerInputActions2;
using Unity.VisualScripting;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;        // Tốc độ khi dash
    public float dashDuration = 0.2f;    // Thời gian dash
    public float dashCooldown = 1f;      // Thời gian chờ sau khi dash
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    [Header("Effects")]
    public ParticleSystem runEffect;
    public ParticleSystem landEffect;
    public ParticleSystem jumpEffect;
    public ParticleSystem dashEffect;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 inputAxis;
    private bool isGrounded;
    private bool wasGroundedLastFrame = true;
    private bool wasInAir = false;
    private bool isRunning;
    private bool isRunningEffectPlaying = false;
    private Animator animator;

    public int jumpCount = 0;
    public int maxJumps = 1;
    public int health = 3;
    private PlayerInputActions3 inputActions;

    public bool isGameOver = false;
    public float StatTimer = 0f;
    public float StatTimer2 = 0f;
    public float StatDuration = 2f; // Thời gian hiệu lực của stat
    public float RandomWalkSpeed = Random.Range(5f, 7f);
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        inputActions = new PlayerInputActions3();
        inputActions.Player.Enable();
        RandomWalkSpeed = Random.Range(5f, 7f);
    }

    private void Update()
    {
        // Cooldown giảm dần
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        bool isJumpPressedThisFrame = inputActions.Player.Jump.triggered;
        bool isDashPressedThisFrame = Input.GetKeyDown(KeyCode.F);

        // Bắt đầu dash
        if (!isDashing && dashCooldownTimer <= 0 && isDashPressedThisFrame && inputAxis.magnitude > 0.1f)
        {
            StartDash();
        }

        if (isDashing)
        {
            DashMovement();
            return; // Không xử lý di chuyển thường khi đang dash
        }

        inputAxis = inputActions.Player.Move.ReadValue<Vector2>();
        isRunning = inputActions.Player.Sprint.IsPressed();

        if (isGrounded)
        {
            jumpCount = 0;
        }

        GetInput();
        HandleMovement();
        HandleGravity();
        HandleJump(isJumpPressedThisFrame);
        HandleEffects();
        UpdateAnimator();

        if(health <= 0)
        {
            isGameOver = true;
            Debug.Log("Game Over! Player has no health left.");
            // Có thể thêm logic để kết thúc trò chơi hoặc reset
        }


        StatTimer += Time.deltaTime;
        if (StatTimer >= StatDuration)
        {
            walkSpeed = RandomWalkSpeed;
            StatTimer2 += Time.deltaTime;
            if (StatTimer2 >= 3f)
            {
                walkSpeed = 4f;
            }
        }


        if(Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack");
        }
    }


    

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        // Hướng dash theo hướng di chuyển hiện tại (camera-relative)
        Vector3 moveDir = GetMoveDirection(new Vector3(inputAxis.x, 0, inputAxis.y));
        dashDirection = moveDir;

        if (dashEffect != null)
            dashEffect.Play();
    }

    private void DashMovement()
    {
        controller.Move(dashDirection * dashSpeed * Time.deltaTime);
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0)
        {
            isDashing = false;
        }
    }

    private void GetInput()
    {
        inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void RotateTowardsMoveDirection(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private Vector3 GetMoveDirection(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        return moveDir.normalized;
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(inputAxis.x, 0f, inputAxis.y).normalized;
        Vector3 move = Vector3.zero;

        if (direction.magnitude >= 0.1f)
        {
            RotateTowardsMoveDirection(direction);
            float speed = isRunning ? runSpeed : walkSpeed;
            Vector3 moveDir = GetMoveDirection(direction);
            move = moveDir * speed;
        }

        move.y = velocity.y;
        controller.Move(move * Time.deltaTime);
    }

    private void HandleGravity()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!isGrounded)
        {
            wasInAir = true;
        }

        velocity.y += gravity * Time.deltaTime;
    }

    private void HandleJump(bool isJumpPressed)
    {
        if (isJumpPressed)
        {
            animator.SetBool("Jump", true);
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount = 1;
                if (jumpEffect != null)
                    jumpEffect.Play();
            }
            else if (jumpCount < maxJumps)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount++;
                if (jumpEffect != null)
                    jumpEffect.Play();
            }
        }
    }

    private void HandleEffects()
    {
        HandleRunEffect();

        if (!wasGroundedLastFrame && isGrounded && wasInAir)
        {
            if (landEffect != null)
                landEffect.Play();
            wasInAir = false;
        }

        wasGroundedLastFrame = isGrounded;
    }

    private void HandleRunEffect()
    {
        bool isRunMoving = inputAxis.magnitude > 0.1f && isRunning;
        if (runEffect != null)
        {
            if (isRunMoving && !isRunningEffectPlaying)
            {
                runEffect.Play();
                isRunningEffectPlaying = true;
            }
            else if (!isRunMoving && isRunningEffectPlaying)
            {
                runEffect.Stop();
                isRunningEffectPlaying = false;
            }
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        float inputMagnitude = inputAxis.magnitude;
        float state = inputMagnitude > 0.1f ? 1f : 0f;
        float vertValue = 0f;

        if (inputMagnitude > 0.1f)
        {
            vertValue = isRunning ? 1f : 0.5f;
        }

        animator.SetFloat("State", state);
        animator.SetFloat("Vert", vertValue, 0.1f, Time.deltaTime);
    }
}
