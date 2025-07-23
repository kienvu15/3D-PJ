using EazyCamera.Legacy;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float rotationSpeed = 10f; // Tốc độ xoay mượt
    public Transform cameraTransform;

    [Header("Effects")]
    public ParticleSystem runEffect;
    public ParticleSystem landEffect;
    public ParticleSystem jumpEffect;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 inputAxis;
    private bool isGrounded;
    private bool wasGroundedLastFrame = true;
    private bool wasInAir = false;
    private bool isRunning;
    private bool isRunningEffectPlaying = false;
    private Animator animator;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GetInput();
        HandleMovement();
        HandleGravity();
        HandleJump();
        HandleEffects();
        UpdateAnimator();
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

        // Gộp chuyển động ngang và trọng lực
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

        // Đánh dấu đã rơi khỏi mặt đất
        if (!isGrounded)
        {
            wasInAir = true;
        }

        velocity.y += gravity * Time.deltaTime;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (jumpEffect != null)
                jumpEffect.Play();
        }
    }

    private void HandleEffects()
    {
        HandleRunEffect();

        // Chỉ chơi land effect nếu trước đó thực sự đã ở trên không
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
        // Chỉ chạy hiệu ứng khi đang trên mặt đất, nhấn chạy, và đang di chuyển
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

        // Lấy giá trị đầu vào (Vector2 từ InputSystem hoặc WASD)
        float inputMagnitude = inputAxis.magnitude;

        // State = 0: idle, State = 1: di chuyển (walk hoặc run)
        float state = inputMagnitude > 0.1f ? 1f : 0f;

        // Vert = tốc độ tương đối (0 idle, 0.5 walk, 1 run)
        float vertValue = 0f;
        if (inputMagnitude > 0.1f)
        {
            vertValue = isRunning ? 1f : 0.5f;
        }

        // Gán vào Animator
        animator.SetFloat("State", state);
        animator.SetFloat("Vert", vertValue, 0.1f, Time.deltaTime); // damping 0.1
    }
}
