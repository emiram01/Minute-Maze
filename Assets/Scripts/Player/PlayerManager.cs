using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public InputManager input;
    public CameraManager cam;
    public Sway sway;
    [Space]
    public PlayerAnimator animator;
    public PlayerMovement movement;

    [Header("Player States")]
    public bool isGrounded;
    public bool isRunning;
    public bool isCrouching;
    [Space]
    public bool canMove;
    public bool canKick;

    private void Start()
    {
        canMove = true;
        canKick = true;
    }

    private void Update()
    {
        PlayerState();
    }

    public void LockPlayer(bool b)
    {
        animator.Idle();
        isGrounded = true;
        canMove = b;
        cam.canMoveCam = b;
        sway.enabled = b;
    }

    private void PlayerState()
    {
        if(isGrounded)
            animator.Grounded();
        else
            animator.NotGrounded();

        movement.CheckCollision();
        movement.CheckMovementInput();
    }
}
