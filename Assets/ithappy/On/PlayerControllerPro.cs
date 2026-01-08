using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerPro : NetworkBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 10f;
    public float gravity = -9.81f;
    private float verticalVelocity = 0f;

    private bool isJumping = false;

    public CharacterController characterController;
    public NetworkMecanimAnimator networkMecanimAnimator;
    public Kien inputActions;

    public override void Spawned()
    {
        inputActions = new Kien();
        inputActions.Enable();
        inputActions.Player.Jump.started += JumpRequest;
    }

    public void JumpRequest(InputAction.CallbackContext context)
    {
        isJumping = true;
    }

    public override void FixedUpdateNetwork()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        bool isRunning = moveDir.magnitude > 0.1f;
        networkMecanimAnimator.Animator.SetBool("Walk", isRunning);

        if (characterController.isGrounded)
        {
            if (isJumping)
            {
                verticalVelocity = jumpForce;
                isJumping = false;
            }
            else
            {
                verticalVelocity = 0f;
            }
        }
        else
        {
            verticalVelocity += gravity * Runner.DeltaTime;
        }

        if(moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);
        }

        Vector3 finalMove = moveDir * moveSpeed + Vector3.up * verticalVelocity;
        characterController.Move(finalMove * Runner.DeltaTime);
    }
}
