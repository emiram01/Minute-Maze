using UnityEngine;

public class Sway : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;
    private CameraManager _cam;
    
    [Header("Position")]
    [SerializeField] private float _amount;
    [SerializeField] private float _maxAmount;
    [SerializeField] private float _smoothAmount;

    [Header("Rotation")]
    [SerializeField] private float _rotation;
    [SerializeField] private float _maxRotation;
    [SerializeField] private float _smoothRotation;

    [Space]
    [SerializeField] private bool _rotationX;
    [SerializeField] private bool _rotationY;
    [SerializeField] private bool _rotationZ;

    private Vector3 _intialPos;
    private Quaternion _initialRot;

    private float _inputX;
    private float _inputY;

    private void Start()
    {
        _cam = _player.cam;

        _intialPos = transform.localPosition;
        _initialRot = transform.localRotation;
    }

    private void Update()
    {
        CalculateSway();
        MoveSway();
        TiltSway();
    }

    private void CalculateSway()
    {
        _inputX = -_cam.mouseX;
        _inputY = -_cam.mouseY;
    }

    private void MoveSway()
    {
        float moveX = Mathf.Clamp(_inputX * _amount, -_maxAmount, _maxAmount);
        float moveY = Mathf.Clamp(_inputY * _amount, -_maxAmount, _maxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + _intialPos, Time.deltaTime * _smoothAmount);
    }

    private void TiltSway()
    {
        float tiltY = Mathf.Clamp(_inputX * _rotation, -_maxRotation, _maxRotation);
        float tiltX = Mathf.Clamp(_inputY * _rotation, -_maxRotation, _maxRotation);

        Vector3 rotate = new Vector3(
            _rotationX ? -tiltX : 0f, 
            _rotationY ? tiltY : 0f, 
            _rotationZ ? tiltY : 0f);

        Quaternion finalRot = Quaternion.Euler(rotate);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot * _initialRot, Time.deltaTime * _smoothRotation);
    }
}
