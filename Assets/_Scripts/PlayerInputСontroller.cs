using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputСontroller : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    
    private InputAction _mousePositionAction;
    private InputAction _deltaAction;
    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _dropAction;
    private InputAction _numAction;
    private InputAction _scrollAction;

    private Vector2 _currentPosition;
    private Vector2 _currentDelta;
    private Vector2 _currentDirection;

    public Vector2 CurrentWorldPosition => Camera.main.ScreenToWorldPoint(_currentPosition);
    public Vector2 Delta => _currentDelta;
    public Vector2 Direction => _currentDirection;

    public Action<Vector2> OnInteract;
    public Action OnDrop;
    public Action<int> OnSelectNum;
    public Action<int> OnScroll;

    private void OnEnable()
    {
        _mousePositionAction = _playerInput.actions["MousePosition"];
        _deltaAction = _playerInput.actions["Look"];
        _moveAction = _playerInput.actions["Move"];
        _interactAction = _playerInput.actions["Interact"];
        _dropAction = _playerInput.actions["Drop"];
        _numAction = _playerInput.actions["SelectNum"];
        _scrollAction = _playerInput.actions["Scroll"];

        _mousePositionAction.performed += context => { _currentPosition = context.ReadValue<Vector2>(); };
        _deltaAction.performed += context => { _currentDelta = context.ReadValue<Vector2>(); };
        _deltaAction.canceled += context => { _currentDelta = context.ReadValue<Vector2>(); };
        _moveAction.performed += context => { _currentDirection = context.ReadValue<Vector2>(); };
        _interactAction.performed += context => { OnInteract?.Invoke(CurrentWorldPosition); };
        _dropAction.performed += context => { OnDrop?.Invoke(); };
        _numAction.performed += OnPressNum;
        _scrollAction.performed += context => { OnScroll?.Invoke((int)context.ReadValue<Vector2>().normalized.y); };
    }

    private void OnPressNum(InputAction.CallbackContext context)
    {
        if (int.TryParse(context.control.name, out var value))
        {
            OnSelectNum?.Invoke(value);
        }
    }

    private void OnDisable()
    {
        _mousePositionAction.performed -= context => { _currentPosition = context.ReadValue<Vector2>(); };
        _deltaAction.performed -= context => { _currentDelta = context.ReadValue<Vector2>(); };
        _moveAction.performed -= context => { _currentDirection = context.ReadValue<Vector2>(); };
        _interactAction.performed -= context => { OnInteract?.Invoke(CurrentWorldPosition); };
        _numAction.performed -= OnPressNum;
        _scrollAction.performed -= context => { OnScroll?.Invoke((int)context.ReadValue<Vector2>().normalized.y); };
    }
}
