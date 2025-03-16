using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Header("Settings")]
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;
    [SerializeField] private float _minRotationX;
    [SerializeField] private float _maxRotationX;
    [SerializeField] private float _smoothTime = 0.05f;
    
    private bool _isActiv;
    private float _xRotation;
    private float _yRotation;
    private float _currentXRotation;
    private float _currentYRotation;    
    private Vector2 _rotationVelocity;

    public float YRotation => _currentYRotation;

    private PlayerInputСontroller Input => _player.Input;

    private void Start()
    {
        SetActiv(true);
        
        _currentXRotation = _xRotation;
        _currentYRotation = _yRotation;
    }
    
    private void Update()
    {
        if (!_isActiv) return;

        UpdateRotation();
    }

    public void SetActiv(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.Confined;
        _isActiv = value;
    }

    private void UpdateRotation()
    {
        float mouseX = Input.Delta.x * _sensX;
        float mouseY = Input.Delta.y * _sensY;

        _yRotation += mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minRotationX, _maxRotationX);

        _currentXRotation = Mathf.SmoothDamp(_currentXRotation, _xRotation, ref _rotationVelocity.y, _smoothTime);
        _currentYRotation = Mathf.SmoothDamp(_currentYRotation, _yRotation, ref _rotationVelocity.x, _smoothTime);

        transform.rotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0);
    }
}
