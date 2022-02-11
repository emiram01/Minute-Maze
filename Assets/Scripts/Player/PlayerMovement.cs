using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;
    [SerializeField] private CharacterController _controller;
    private InputManager _input;
    private CameraManager _cam;
    private PlayerAnimator _animator;

    [Header("Player Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance;
    [SerializeField] private float _gravity;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _momentumDrag;
    private float _moveSpeed;

    [Header("Run")]
    [SerializeField] private float _runSpeed;

    [Header("Crouch")]
    [SerializeField] private Transform _crouchGroundCheck;
    [SerializeField] private float _crouchHeight;
    [SerializeField] private float _crouchSpeed;
    private Transform _normalGroundCheck;
    private float _normalHeight;
    
    [Header("Jump")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpCooldown;
    private bool _jumping;

    //Input and Vectors
    private Vector3 _velocity;
    private Vector3 _initialVelocity;
    private Vector3 _currentMomentum;
    private float h, v;

    private void Start()
    {
        _input = _player.input;
        _cam = _player.cam;
        _animator = _player.animator;

        _moveSpeed = _walkSpeed;
        _normalHeight = _controller.height;
        _normalGroundCheck = _groundCheck;
        _initialVelocity = _velocity;
    }
 
    public void CheckCollision()
    {
        if(!_jumping)
            _player.isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundLayer);
        
        if(_player.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;
    }

    #region General Movement
    public void CheckMovementInput()
    {
        h = _input.xInput;
        v = _input.yInput;

        if(_player.canMove)
        {
            Move();
            CheckDirection();
            CheckJump();
            CheckRun();
            CheckCrouch();
        }
        else
        {
            EndRun();
            EndCrouch();
        }
    }

    public void Grapple(Vector3 dir, float speed, float multiplier)
    {
        _controller.Move(dir * speed * multiplier * Time.deltaTime);
    }

    public void ResetGravity()
    {
        _velocity.y = 0f;
    }

    private void Move()
    {
        Vector3 move = transform.right * h + transform.forward * v;

        _controller.Move(move * _moveSpeed * Time.deltaTime);
        
        _velocity.y += _gravity * Time.deltaTime;
        
        _velocity += _currentMomentum;

        _controller.Move(_velocity * Time.deltaTime);

        if(_currentMomentum.magnitude >= 0f)
        {
            _velocity -= _currentMomentum;
            _currentMomentum -= _currentMomentum * _momentumDrag * Time.deltaTime;
            
            if(_currentMomentum.magnitude < 0.0f)
            {
                _currentMomentum = Vector3.zero;
                _cam.ChangeFov(_cam.originalFov);
            }   
        }
    }

    private void CheckDirection()
    {
        if(v == 1)
        {
            _animator.WalkingForward();
        }
        else if(v == -1)
        {
            _animator.WalkingBack();
        }
        else if(h == -1)
        {
            _animator.WalkingLeft();
        }
        else if(h == 1)
        {
            _animator.WalkingRight();
        }

        if(h == 0 && v == 0)
        {
            _animator.Idle();

            if(!_player.isCrouching)
            {
                if(_cam.mouseX < -0.5f)
                    _animator.TurningLeft();
                else if(_cam.mouseX > 0.5f)
                    _animator.TurningRight();
            }   
        }
    }
    #endregion

    #region Jump
    public void Jump()
    {
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    private void CheckJump()
    {
        if(_input.jumpInput && !_jumping && _player.isGrounded)
        {
            _jumping = true;
            _player.isGrounded = false;
            Invoke(nameof(ResetJump), _jumpCooldown);
            Jump();
        }

        if(_player.isGrounded)
            ResetJump();
    }

    private void ResetJump()
    {
        _jumping = false;
    }
    #endregion

    #region Run
    private void CheckRun()
    {
        if(_input.runInput)
        {
            if(v != 0 && !_player.isCrouching)
                StartRun();
                
            if(v == 0)
                EndRun();
        }
        else
            EndRun();
    }

    private void StartRun()
    {
        _player.isRunning = true;
        _moveSpeed = _runSpeed;
        _animator.Running();
        _cam.ChangeFov(_cam.originalFov + 10f);
    }

    public void EndRun()
    {
        _input.runInput = false;
        _player.isRunning = false;
        _moveSpeed = _walkSpeed;
        _animator.NotRunning();
        _cam.ChangeFov(_cam.originalFov);
    }
    #endregion

    #region Crouch
    private void CheckCrouch()
    {
        if(_input.crouchInput && !_player.isRunning)
            StartCrouch();
        else if(_player.isCrouching)
            EndCrouch();
    }

    private void StartCrouch()
    {
        _player.isCrouching = true;
        _moveSpeed = _crouchSpeed;
        _groundCheck = _crouchGroundCheck;
        _controller.height = _crouchHeight;

        if(v != 0 || h != 0)
            _animator.Crouching();
        else
            _animator.NotCrouching();
    }

    private void EndCrouch()
    {
        _player.isCrouching = false;
        _moveSpeed = _walkSpeed;
        _groundCheck = _normalGroundCheck;
        _controller.height = _normalHeight;
        _animator.NotCrouching();
    }
    #endregion
}