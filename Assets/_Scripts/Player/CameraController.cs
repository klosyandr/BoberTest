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
    private float _sensCoef = 1;

    public float YRotation => _currentYRotation;

    private PlayerInputСontroller Input => _player.Input;

    private void Start()
    {
        _player.Input.OnChangeSens += ChangeSensCoef;
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

    public void ChangeSensCoef(float value)
    {
        _sensCoef = Mathf.Clamp(_sensCoef + value * 0.1f, 0.1f, 1.2f);
    }

    private void UpdateRotation()
    {
        float mouseX = Input.Delta.x * _sensX * _sensCoef;
        float mouseY = Input.Delta.y * _sensY * _sensCoef;

        _yRotation += mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minRotationX, _maxRotationX);

        _currentXRotation = Mathf.SmoothDamp(_currentXRotation, _xRotation, ref _rotationVelocity.y, _smoothTime);
        _currentYRotation = Mathf.SmoothDamp(_currentYRotation, _yRotation, ref _rotationVelocity.x, _smoothTime);

        transform.rotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0);
    }
}