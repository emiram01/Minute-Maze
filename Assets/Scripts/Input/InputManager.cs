using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] ControlSettings _settings;
    private ActionInput _input;
    private Keyboard _kb;

    private Vector2 _movementInput;
    private Vector2 _cameraInput;

    public float xInput, yInput;
    public float xCameraInput, yCameraInput;
    public bool runInput;
    public bool crouchInput;
    public bool jumpInput;
    public bool pause;

    private void OnEnable()
    {
        if(_input == null)
        {
            _input = new ActionInput();

           _input.Player.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
           _input.Player.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();

           _input.Player.Run.performed += i => runInput = true;
           _input.Player.Run.canceled += i => runInput = false;

           _input.Player.Crouch.performed += i => crouchInput = true;
           _input.Player.Crouch.canceled += i => crouchInput = false;

           _input.Player.Jump.performed += i => jumpInput = true;

           _input.Player.Pause.performed += i => pause = true;
        }
       _input.Enable();
    }

    private void OnDisable()
    {
       _input.Disable();
    }

    private void Start()
    {
        _kb = InputSystem.GetDevice<Keyboard>();
    }

    private void Update()
    {
        GetInput();
    }

    private void LateUpdate()
    {
        ResetInput();
    }

    private void GetInput()
    {
        xInput = _movementInput.x;
        yInput = _movementInput.y;

        xCameraInput = _cameraInput.x;
        yCameraInput = _cameraInput.y;
    }

    private void ResetInput()
    {
        jumpInput = false;
        pause = false;
    }
}
