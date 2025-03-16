using NaughtyAttributes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [ReadOnly] [SerializeField] private Player _player;
    [Header("Movement")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _speed = 2f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        _orientation.rotation = Quaternion.Euler(0, _player.Camera.YRotation, 0);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = CalculateDirection() * _speed;
    }

    private Vector3 CalculateDirection()
    {
        return _orientation.right * _player.Input.Direction.x + _orientation.forward * _player.Input.Direction.y;
    }
}