using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;
    [SerializeField] private Camera _camera;
    private InputManager _input;

    [Header("Field of View")]
    [Range(20f, 150f)] [SerializeField] private float _fov;
    [SerializeField] private float _fovTime;
    public float originalFov;

    [Header("Mouse Sensitivity")]
    [SerializeField] [Range(0f, .2f)] private float _sensitivity;
    public bool canMoveCam;
    public float mouseX;
    public float mouseY;

    [Header("Rotations")]
    [SerializeField] private float _camRotation;
    [SerializeField] private float _rotateTime;
    private float _xRotation;
    private float _yRotation;
    private float _zRotation;

    [Header("Position")]
    [SerializeField] private Transform _playerTrans;
    public bool mazeView;
    private Transform _trans;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mazeView = true;
    }
    
    private void Start()
    {
        _input = _player.input;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canMoveCam = true;

        originalFov = _fov;
        _trans = transform;
        _zRotation = 0f;
    }

    private void LateUpdate()
    {
        if(!mazeView)
            CameraMovement();
    }

    public void ChangeProjection(bool b)
    {
        _camera.orthographic = b;
    }
    
    private void CameraMovement()
    {
        mouseX = _input.xCameraInput * _sensitivity;
        mouseY = _input.yCameraInput * _sensitivity;

        if(canMoveCam)
            RotateCamera();
        
        PositionCamera();
    }

    private void RotateCamera()
    {
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -89f, 65f);

        Vector3 rot = _trans.localRotation.eulerAngles;
        _yRotation = rot.y + mouseX;
        
        _trans.localRotation = Quaternion.Euler(_xRotation, _yRotation, _zRotation);
        _playerTrans.localRotation = Quaternion.Euler(0, _yRotation, 0);
    }

    private void PositionCamera()
    {
        _trans.position = _playerTrans.position;
    }

    public void ChangeFov(float newFov)
    {
        _fov = Mathf.Lerp(_fov, newFov, _fovTime * Time.deltaTime);
        _camera.fieldOfView = _fov;
    }
}