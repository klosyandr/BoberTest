using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerInputСontroller  _input;
    [Space]
    [SerializeField] private Transform _cameraPos;
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;
    [SerializeField] private Vector2 _minDelta = Vector2.one;
    [SerializeField] private float _minRotationX;
    [SerializeField] private float _maxRotationX;
    
    private float _xRotation;
    private float _yRotation;

    private bool _isActiv;

    private void Start()
    {
        SetActiv(true);
    }
    
    private void Update()
    {
        if (!_isActiv) return;

        float mouseX = Mathf.Abs(_input.Delta.x) >= _minDelta.x ? _input.Delta.x * Time.deltaTime * _sensX : 0;
        float mouseY = Mathf.Abs(_input.Delta.x) >= _minDelta.y ? _input.Delta.y * Time.deltaTime * _sensY : 0;

        _yRotation += mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minRotationX, _maxRotationX);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        transform.position = _cameraPos.position;
        
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    public void SetActiv(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.Confined;
        _isActiv = value;
    }

}
