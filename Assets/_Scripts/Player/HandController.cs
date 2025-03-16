using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [ReadOnly] [SerializeField] private Player _player;
    [Space]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _hidePosition;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private float _forceDrop;

    public Transform TargetPos => _targetPosition;

    private TakeComponent _selectedObject;

    public TakeComponent SelectedObject
    {
        get { return _selectedObject; }
        set
        {
            if (_selectedObject != null) Hide(_selectedObject);

            _selectedObject = value;

            if (_selectedObject != null) Show(_selectedObject);
        }
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.Input.OnDrop += OnDrop;
    }

    private void Hide(TakeComponent target)
    {
        target.DOKill(true);
        target.transform.DOLocalMove(_hidePosition.localPosition, _duration).OnKill(() => target.gameObject.SetActive(false));
    }

    private void Show(TakeComponent target)
    {
        target.DOKill(true);
        target.gameObject.SetActive(true);
        target.gameObject.transform.DOLocalMove(target.LocalOffset, _duration);
    }

    private void OnDrop()
    {
        if (SelectedObject == null) return;
        
        var dropable = SelectedObject;
        _player.Inventory.Remove(SelectedObject.Tag, SelectedObject);
        dropable.Drop(_orientation.forward * _forceDrop);
    }

    private void OnDestroy()
    {
        _player.Input.OnDrop -= OnDrop;
    }
}