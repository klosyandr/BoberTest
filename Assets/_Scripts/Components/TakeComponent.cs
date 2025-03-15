using UnityEngine;

public class TakeComponent : IInteractableComponent
{
    [InventoryId] [SerializeField] public string _tag;
    [SerializeField] private float _sizeCoef = 1;

    private Vector3 _initialScale;
    private Rigidbody _rigidbody;
    private Collider _collider;

    public string Tag => _tag;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _initialScale = transform.localScale;
    }
    
    public override void Interact(Player player)
    {
        if (!player.Inventory.TryAdd(_tag, this)) return;
        
        Take(player.TargetPos);
    }

    public void Take(Transform parent)
    {
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        transform.SetParent(parent);

        transform.localScale = _initialScale * _sizeCoef;
        transform.localRotation =  Quaternion.Euler(0, 0, 0);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
    
    public void Drop(Vector3 force)
    {
        gameObject.SetActive(true);

        transform.localScale = _initialScale;
        transform.parent = null;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }
}


