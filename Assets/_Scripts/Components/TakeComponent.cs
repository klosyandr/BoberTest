using DG.Tweening;
using UnityEngine;

public class TakeComponent : IInteractableComponent
{
    [InventoryId] [SerializeField] public string _tag;
    [SerializeField] private float _sizeCoef = 1;
    [SerializeField] private Vector3 _localOffset = Vector3.zero;
    [SerializeField] private AudioSource _sfx;

    private Vector3 _initialScale;
    private Rigidbody _rigidbody;
    private Collider _collider;

    public string Tag => _tag;
    public Vector3 LocalOffset => _localOffset;

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
        player.Hand.Take(this, _sfx.clip);
    }

    public void Take(Transform parent)
    {
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        transform.SetParent(parent);

        transform.localScale = _initialScale * _sizeCoef;
        transform.localRotation =  Quaternion.Euler(0, 0, 0);
        transform.localPosition = _localOffset;
        gameObject.SetActive(false);
    }
    
    public void Drop(Vector3 force)
    {
        transform.parent = null;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(force, ForceMode.Impulse);
        transform.DOScale(_initialScale, 0.2f);
        _sfx?.Play();
    }
}


