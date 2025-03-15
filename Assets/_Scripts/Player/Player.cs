using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputСontroller _input;
    [Header("Movement")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _speed = 2f;
    [Header("Take")]
    [SerializeField] private Transform _targetPos;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _distance = 3f;
    [SerializeField] private float _forceDrop;

    private TakeComponent _tempTarget;

    private Rigidbody _rigidbody;
    private Inventory _inventory;

    public Inventory Inventory => _inventory;
    public Transform TargetPos => _targetPos;
    public TakeComponent TempTarget
    {
        set
        {
            _tempTarget?.gameObject.SetActive(false);
            _tempTarget = value;
            _tempTarget?.gameObject.SetActive(true);
        }
        get { return _tempTarget; }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;

        _inventory = GetComponent<Inventory>();

        _input.OnInteract += OnInteract;
        _input.OnDrop += OnDrop;
        _input.OnSelectNum += _inventory.SelectIndex;
        _input.OnScroll += _inventory.SetNextItem;

        _inventory.OnIndexChanged += (int value, int _) => TempTarget = value == -1 ? null : _inventory.Items[value].objectPool[0];
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = CalculateDirection() * _speed;
    }

    private Vector3 CalculateDirection()
    {
        return _orientation.right * _input.Direction.x + _orientation.forward * _input.Direction.y;
    }

    private void OnInteract(Vector2 vector)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _distance);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, _distance, _layer))
        {
            var target = hit.collider.gameObject.GetComponent<IInteractableComponent>();
            if (target != null)
            {
                target.Interact(this);
            }
        }
    }
    
    private void OnDrop()
    {
        if (_tempTarget == null) return;
        
        var dropable = _tempTarget;
        _inventory.Remove(_tempTarget.Tag, _tempTarget);
        dropable.Drop(_orientation.forward * _forceDrop);
    }

    private void OnDestroy()
    {
        _input.OnInteract -= OnInteract;
        _input.OnDrop -= OnDrop;
        _input.OnSelectNum -= _inventory.SelectIndex;
        _input.OnScroll -= _inventory.SetNextItem;
    }
}